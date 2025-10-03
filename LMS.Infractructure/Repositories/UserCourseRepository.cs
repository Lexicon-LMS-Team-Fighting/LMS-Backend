using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories
{
    /// <summary>
    /// Repository implementation for managing <see cref="UserCourse"/> entities.
    /// </summary>
    public class UserCourseRepository : RepositoryBase<UserCourse>, IUserCourseRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserCourseRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The database context to be used by the repository.</param>
        public UserCourseRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Deletes all <see cref="UserCourse"/> entries associated with a specific user.
        /// </summary>
        /// <param name="studentId">The unique identifier of the user whose enrollments are being deleted.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public async Task DeleteAllByUserId(string studentId)
        {
            var enrollments = await FindAll()
                .Where(enrollment => enrollment.UserId == studentId)
                .ToListAsync();

            if (!enrollments.Any())
                return;
            
            DeleteRange(enrollments);
        }
    }
}
