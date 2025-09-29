using Bogus;
using LMS.Infractructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Services;

/// <summary>
/// A hosted service responsible for seeding the database with initial data.
/// This service is executed when the application starts in a development environment.
/// It ensures that the database is populated with roles, users, courses, modules, activities, and other related data.
/// </summary>

//Add in secret.json
//{
//   "password" :  "YourSecretPasswordHere"
//}
public class DataSeedHostingService : IHostedService
{
    // Constants for seeding data
    private const int TeachersCount = 5; // Number of teachers to generate
    private const int StudentsCount = 30; // Number of students to generate
    private const int CoursesCount = 10; // Number of courses to generate
    private const int MinModulesPerCourse = 1; // Minimum number of modules per course. An exact number is randomly chosen between Min and Max
    private const int MaxModulesPerCourse = 5; // Maximum number of modules per course. An exact number is randomly chosen between Min and Max
    private const int MinActivitiesPerModule = 1; // Minimum number of activities per module. An exact number is randomly chosen between Min and Max
    private const int MaxActivitiesPerModule = 10; // Maximum number of activities per module. An exact number is randomly chosen between Min and Max
    private const int DocumentsCount = 50; // Number of documents to generate
    private const string DefaultTeacherUserName = "Teacher"; // Default username for teachers
    private const string DefaultStudentUserName = "Student"; // Default username for students
    private const string DefaultTeacherEmail = "teacher@test.com"; // Default email for teachers
    private const string DefaultStudentEmail = "student@test.com"; // Default email for students
    private const string TeacherRole = "Teacher"; // Role name for teachers
    private const string StudentRole = "Student"; // Role name for students

    private readonly IServiceProvider serviceProvider;
    private readonly IConfiguration configuration;
    private readonly ILogger<DataSeedHostingService> logger;
    private UserManager<ApplicationUser> userManager = null!;
    private RoleManager<IdentityRole> roleManager = null!;
    private ApplicationDbContext context = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataSeedHostingService"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for resolving scoped services.</param>
    /// <param name="configuration">The application configuration for accessing settings.</param>
    /// <param name="logger">The logger for logging information and errors.</param>
    public DataSeedHostingService(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<DataSeedHostingService> logger)
    {
        this.serviceProvider = serviceProvider;
        this.configuration = configuration;
        this.logger = logger;
    }

    /// <summary>
    /// Starts the data seeding process when the application starts.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Uncomment to clear the database and apply migrations
        //await ClearDatabaseAsync(cancellationToken);

        if (!env.IsDevelopment()) return; // Only seed data in development environment
        if (await context.Users.AnyAsync(cancellationToken)) return;  // Only seed data if no users exist 

        ArgumentNullException.ThrowIfNull(roleManager, nameof(roleManager));
        ArgumentNullException.ThrowIfNull(userManager, nameof(userManager));

        try
        {
            // Populate the database with initial data
            await SeedDatabaseAsync(cancellationToken);

            logger.LogInformation("Seed complete");
        }
        catch (Exception ex)
        {
            logger.LogError($"Data seed failed with error: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Clears the database by deleting all data and applying migrations.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    private async Task ClearDatabaseAsync(CancellationToken cancellationToken)
    {
        if (await context.Database.EnsureDeletedAsync(cancellationToken))
        {
            logger.LogInformation("Database has been deleted.");
        }
        else
        {
            logger.LogInformation("Database does not exist or could not be deleted.");
        }

        await context.Database.MigrateAsync(cancellationToken);
        logger.LogInformation("Database has been recreated and migrations applied.");
    }

    /// <summary>
    /// Populates the database with roles, users, courses, modules, activities, and other related data.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    private async Task SeedDatabaseAsync(CancellationToken cancellationToken)
    {
        await AddRolesAsync([TeacherRole, StudentRole]);
        await AddTeachersAsync();

        var students = await AddStudentsAsync();
        var courses = await AddCoursesAsync(cancellationToken);
        await context.SaveChangesAsync();
        
        await AddEnrollmentsAsync(students, courses, cancellationToken);

        var modules = await AddModulesAsync(courses, cancellationToken);
        var activityTypes = await AddActivityTypesAsync(cancellationToken);
        var activities = await AddLMSActivitiesAsync(modules, activityTypes, cancellationToken);
        await AddDocumentsAsync(students, courses, modules, activities, cancellationToken);
        await AddFeedbacksAsync(activities, students, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Adds roles to the database if they do not already exist.
    /// </summary>
    /// <param name="rolenames">An array of role names to add.</param>
    private async Task AddRolesAsync(string[] rolenames)
    {
        foreach (string rolename in rolenames)
        {
            if (await roleManager.RoleExistsAsync(rolename)) continue;
            var role = new IdentityRole { Name = rolename };
            var res = await roleManager.CreateAsync(role);

            if (!res.Succeeded) throw new Exception(string.Join("\n", res.Errors));
        }
    }

    /// <summary>
    /// Generates and adds courses to the database.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    private async Task<IEnumerable<Course>> AddCoursesAsync(CancellationToken cancellationToken)
    {
        if (await context.Courses.AnyAsync(cancellationToken))
            throw new Exception("Courses already exist in the database.");

        var faker = new Faker<Course>("sv").Rules((f, e) =>
        {
            e.Id = Guid.NewGuid();
            e.Name = f.Company.CatchPhrase();
            e.Description = f.Lorem.Paragraph();
            e.StartDate = f.Date.Past(1);
            e.EndDate = f.Date.Future(1, e.StartDate);
        });

        var courses = faker.Generate(CoursesCount);

        await context.Courses.AddRangeAsync(courses, cancellationToken);

        return courses;
    }

    /// <summary>
    /// Generates and adds modules to the database for the given courses.
    /// </summary>
    /// <param name="courses">The courses to add modules to.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    private async Task<IEnumerable<Module>> AddModulesAsync(IEnumerable<Course> courses, CancellationToken cancellationToken)
    {
        if (!courses.Any())
            throw new Exception("No courses available to add modules to.");

        var modules = new List<Module>();

        foreach (var course in courses)
        {
            var faker = new Faker<Module>("sv").Rules((f, e) =>
            {
                e.Id = Guid.NewGuid();
                e.CourseId = course.Id;
                e.Name = f.Company.CatchPhrase();
                e.Description = f.Lorem.Paragraph();
                e.StartDate = f.Date.Between(course.StartDate, course.EndDate);
                e.EndDate = f.Date.Between(e.StartDate, course.EndDate);
            });

            var modulesCount = new Random().Next(MinModulesPerCourse, MaxModulesPerCourse);
            var modulesInCourse = faker.Generate(modulesCount);

            modules.AddRange(modulesInCourse);
        }

        await context.Modules.AddRangeAsync(modules, cancellationToken);

        return modules;
    }

    /// <summary>
    /// Adds predefined activity types to the database.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    private async Task<IEnumerable<ActivityType>> AddActivityTypesAsync(CancellationToken cancellationToken)
    {
        if (await context.ActivityTypes.AnyAsync(cancellationToken))
            throw new Exception("Activity types already exist in the database.");

        var activityTypes = new[]
        {
           "Föreläsning",
           "Seminarium",
           "Laboration",
           "Grupparbete",
           "Inlämningsuppgift",
           "Prov",
           "Workshop",
           "Handledning",
           "Självstudier",
           "Examination"
        };

        var activityTypeEntities = activityTypes.Select(name => new ActivityType
        {
            Id = Guid.NewGuid(),
            Name = name
        }).ToList();

        await context.ActivityTypes.AddRangeAsync(activityTypeEntities, cancellationToken);

        return activityTypeEntities;
    }

    /// <summary>
    /// Generates and adds LMS activities to the database for the given modules and activity types.
    /// </summary>
    /// <param name="modules">The modules to which activities will be added.</param>
    /// <param name="activityTypes">The predefined activity types to assign to activities.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of the added LMS activities.</returns>
    /// <exception cref="Exception">Thrown if no modules are available to add activities to.</exception>
    private async Task<IEnumerable<LMSActivity>> AddLMSActivitiesAsync(IEnumerable<Module> modules, IEnumerable<ActivityType> activityTypes, CancellationToken cancellationToken)
    {
        if (!modules.Any())
            throw new Exception("No modules available to add activities to.");

        var activities = new List<LMSActivity>();

        foreach (var module in modules)
        {
            var faker = new Faker<LMSActivity>("sv").Rules((f, e) =>
            {
                e.Id = Guid.NewGuid();
                e.ModuleId = module.Id;
                e.ActivityTypeId = f.PickRandom(activityTypes).Id;
                e.Name = f.Company.CatchPhrase();
                e.Description = f.Lorem.Paragraph();
                e.StartDate = f.Date.Between(module.StartDate, module.EndDate);
                e.EndDate = f.Date.Between(e.StartDate, module.EndDate);
            });

            var activitiesCount = new Random().Next(MinActivitiesPerModule, MaxActivitiesPerModule);
            var activitiesInModule = faker.Generate(activitiesCount);

            activities.AddRange(activitiesInModule);
        }

        await context.LMSActivities.AddRangeAsync(activities, cancellationToken);

        return activities;
    }

    /// <summary>
    /// Generates and adds feedbacks for LMS activities from students.
    /// </summary>
    /// <param name="activities">The activities to add feedback to.</param>
    /// <param name="students">The students providing the feedback.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns></returns>
    /// <exception cref="Exception">Thrown if no activities or students are available to add feedback to.</exception>
    private async Task<IEnumerable<LMSActivityFeedback>> AddFeedbacksAsync(IEnumerable<LMSActivity> activities, IEnumerable<ApplicationUser> students, CancellationToken cancellationToken)
    {
        if (!activities.Any() || !students.Any())
            throw new Exception("No activities or students available to add feedback to.");

        var feedbackStatuses = new[] { "Genomförd", "Försenad", "Godkänd" };
        var feedbacks = new List<LMSActivityFeedback>();
        var rnd = new Random();

        foreach (var activity in activities)
        {
            // Randomly decide how many feedbacks to create for this activity
            var feedbackCount = rnd.Next(1, Math.Min(5, students.Count())); // Limit to max 5 feedbacks per activity

            // Shuffle students and take the first 'feedbackCount' students to avoid duplicates
            var selectedStudents = students.OrderBy(x => rnd.Next()).Take(feedbackCount).ToList();

            foreach (var student in selectedStudents)
            {
                var faker = new Faker<LMSActivityFeedback>("sv").Rules((f, e) =>
                {
                    e.LMSActivityId = activity.Id;
                    e.UserId = student.Id;
                    e.Feedback = f.Lorem.Sentence();
                    e.Status = f.PickRandom(feedbackStatuses);
                });

                var feedback = faker.Generate();
                feedbacks.Add(feedback);
            }
        }

        await context.LMSActivityFeedbacks.AddRangeAsync(feedbacks, cancellationToken);
        return feedbacks;
    }

    /// <summary>
    /// Generates and adds students to the database.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of the added students.</returns>
    private async Task<IEnumerable<ApplicationUser>> AddStudentsAsync()
    {
        var students = await AddUsersAsync(StudentsCount, StudentRole);

        // Set one student to have the default username and email
        var randomStudent = students.ElementAt(new Random().Next(students.Count()));
        randomStudent.UserName = DefaultStudentUserName;
        randomStudent.NormalizedUserName = DefaultStudentUserName.ToUpper();
        randomStudent.Email = DefaultStudentEmail;
        randomStudent.NormalizedEmail = DefaultStudentEmail.ToUpper();

        return students;
    }

    /// <summary>
    /// Generates and adds teachers to the database.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of the added teachers.</returns>
    private async Task<IEnumerable<ApplicationUser>> AddTeachersAsync()
    {
        if (await context.Users.AnyAsync())
            throw new Exception("Users already exist in the database.");

        var teachers = await AddUsersAsync(TeachersCount, TeacherRole);

        // Set one teacher to have the default username and email
        var randomTeacher = teachers.ElementAt(new Random().Next(teachers.Count()));
        randomTeacher.UserName = DefaultTeacherUserName;
        randomTeacher.NormalizedUserName = DefaultTeacherUserName.ToUpper();
        randomTeacher.Email = DefaultTeacherEmail;
        randomTeacher.NormalizedEmail = DefaultTeacherEmail.ToUpper();


        return teachers;
    }

    /// <summary>
    /// Generates and adds users to the database with the specified role.
    /// </summary>
    /// <param name="count">The number of users to add.</param>
    /// <param name="role">The role to assign to the users.</param>
    private async Task<IEnumerable<ApplicationUser>> AddUsersAsync(int count, string role)
    {
        var password = configuration["password"];
        ArgumentNullException.ThrowIfNull(password, nameof(password));

        var faker = new Faker<ApplicationUser>("sv").Rules((f, e) =>
        {
            e.Id = Guid.NewGuid().ToString();
            e.FirstName = f.Name.FirstName();
            e.LastName = f.Name.LastName();
            e.Email = f.Person.Email;
            e.UserName = f.Person.UserName;
        });

        var users = faker.Generate(count);

        foreach (var user in users)
        {
            // Create user with password
            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

            // Assign role to user
            var roleResult = await userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded) throw new Exception(string.Join("\n", roleResult.Errors));
        }

        return users;
    }

    /// <summary>
    /// Enrolls students into courses.
    /// </summary>
    /// <param name="students">The students to enroll.</param>
    /// <param name="courses">The courses to enroll students into.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <exception cref="Exception">Thrown if students or courses are missing for enrollment.</exception>
    private async Task AddEnrollmentsAsync(IEnumerable<ApplicationUser> students, IEnumerable<Course> courses, CancellationToken cancellationToken)
    {
        if (!students.Any() || !courses.Any())
            throw new Exception("Students or courses are missing for enrollment.");

        var enrollments = new List<UserCourse>();
        var coursesList = courses.ToList();
        var rnd = new Random();
        
        foreach (var student in students)
        {
			var randomIndex = rnd.Next(coursesList.Count);
			
            enrollments.Add(new UserCourse
            {
                UserId = student.Id,
                CourseId = coursesList[randomIndex].Id, // Use the indexed list
            });
        }

        await context.AddRangeAsync(enrollments, cancellationToken);
    }
   

    /// <summary>
    /// Adds documents to the database, associating them with students, courses, modules, and activities.
    /// </summary>
    /// <param name="students">The students to associate with documents.</param>
    /// <param name="courses">The courses to associate with documents.</param>
    /// <param name="modules">The modules to associate with documents.</param>
    /// <param name="activities">The activities to associate with documents.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <exception cref="Exception">Thrown if documents already exist in the database or any required data is missing.</exception>
    private async Task AddDocumentsAsync(IEnumerable<ApplicationUser> students, IEnumerable<Course> courses, IEnumerable<Module> modules, IEnumerable<LMSActivity> activities, CancellationToken cancellationToken)
    {
        if (await context.Documents.AnyAsync(cancellationToken))
            throw new Exception("Documents already exist in the database.");

        if (!students.Any() || !courses.Any() || !modules.Any())
            throw new Exception("Insufficient data to create documents.");

        var faker = new Faker<Document>("sv").Rules((f, e) =>
        {
            e.Id = Guid.NewGuid();
            e.UserId = f.PickRandom(students).Id;

            // Randomly decide which properties to assign, ensuring at least one is non-null
            var assignOptions = new[] { "Course", "Module", "Activity" };
            var selectedOptions = f.Random.ListItems(assignOptions, f.Random.Int(1, assignOptions.Length));

            e.CourseId = selectedOptions.Contains("Course") ? f.PickRandom(courses).Id : null;
            e.ModuleId = selectedOptions.Contains("Module") ? f.PickRandom(modules).Id : null;
            e.ActivityId = selectedOptions.Contains("Activity") ? f.PickRandom(activities).Id : null;

            e.Path = f.System.FilePath();
            e.Name = f.System.FileName();
            e.Description = f.Lorem.Sentence();
            e.TimeStamp = f.Date.Recent(30);
        });

        var documents = faker.Generate(DocumentsCount);

        await context.Documents.AddRangeAsync(documents, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
