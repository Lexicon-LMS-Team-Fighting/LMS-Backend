using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.AspNetCore.Identity;

namespace LMS.Infractructure.Repositories;

/// <summary>
/// Defines a unit of work that encapsulates access to multiple repositories
/// and coordinates changes across them to ensure transactional consistency.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext context;

    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<ICourseRepository> _courseRepository;
    private readonly Lazy<IModuleRepository> _moduleRepository;
    private readonly Lazy<ILMSActivityRepository> _lmsActivityRepository;
    private readonly Lazy<IActivityTypeRepository> _activityTypeRepository;
    private readonly Lazy<IDocumentRepository> _documentRepository;
    private readonly Lazy<ILMSActivityFeedbackRepository> _lmsActivityFeedbackRepository;
    private readonly Lazy<IUserCourseRepository> _userCourseRepository;

    public IUserRepository User => _userRepository.Value;
	public ICourseRepository Course => _courseRepository.Value;
    public IModuleRepository Module => _moduleRepository.Value;
    public ILMSActivityRepository LMSActivity => _lmsActivityRepository.Value;
    public IActivityTypeRepository ActivityType => _activityTypeRepository.Value;
    public IDocumentRepository Document => _documentRepository.Value;
    public ILMSActivityFeedbackRepository LMSActivityFeedback => _lmsActivityFeedbackRepository.Value;
    public IUserCourseRepository UserCourse => _userCourseRepository.Value;

    public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(context, userManager));
        _courseRepository = new Lazy<ICourseRepository>(() => new CourseRepository(context));
        _moduleRepository = new Lazy<IModuleRepository>(() => new ModuleRepository(context));
        _lmsActivityRepository = new Lazy<ILMSActivityRepository>(() => new LMSActivityRepository(context));
        _activityTypeRepository = new Lazy<IActivityTypeRepository>(() => new ActivityTypeRepository(context));
        _documentRepository = new Lazy<IDocumentRepository>(() => new DocumentRepository(context));
        _lmsActivityFeedbackRepository = new Lazy<ILMSActivityFeedbackRepository>(() => new LMSActivityFeedbackRepository(context));
        _userCourseRepository = new Lazy<IUserCourseRepository>(() => new UserCourseRepository(context));
    }

    public async Task CompleteAsync() => await context.SaveChangesAsync();
}
