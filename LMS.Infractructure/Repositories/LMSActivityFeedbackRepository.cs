using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories
{
    /// <summary>
    /// Repository implementation for managing <see cref="LMSActivityFeedback"/> entities.
    /// </summary>
    public class LMSActivityFeedbackRepository : RepositoryBase<LMSActivityFeedback>, ILMSActivityFeedbackRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LMSActivityFeedbackRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The database context to be used by the repository.</param>
        public LMSActivityFeedbackRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public async Task DeleteAllInCourseByUserId(string userId, Guid courseId)
        {
            var feedbacks = await FindAll()
                .Where(f => f.UserId == userId && f.LMSActivity.Module.CourseId == courseId)
                .ToListAsync();

            if (!feedbacks.Any())
                return;
            
            DeleteRange(feedbacks);
        }

        /// <inheritdoc />
        public async Task<LMSActivityFeedback?> GetByActivityAndUserIdAsync(Guid activityId, string userId, bool changeTracking = false)
        {
            return await FindByCondition(f => f.LMSActivityId == activityId && f.UserId == userId, changeTracking)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task<bool> ExistsAsync(Guid activityId, string userId)
        {
            return await FindByCondition(f => f.LMSActivityId == activityId && f.UserId == userId, trackChanges: false)
                .AnyAsync();
        }
    }
}
