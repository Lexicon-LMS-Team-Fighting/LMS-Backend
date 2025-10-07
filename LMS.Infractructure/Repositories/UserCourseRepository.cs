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

        /// <inheritdoc />
        public async Task DeleteAllByUserId(string studentId)
        {
            var enrollments = await FindAll()
                .Where(e => e.UserId == studentId)
                .ToListAsync();

            if (!enrollments.Any())
                return;
            
            DeleteRange(enrollments);
        }
    }
}
