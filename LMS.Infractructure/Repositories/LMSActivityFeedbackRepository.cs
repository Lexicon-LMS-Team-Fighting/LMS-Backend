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

        public async Task<LMSActivityFeedback?> GetByIdAsync(Guid feedbackId, bool changeTracking = false)
        {
            return await FindByCondition(f => f.Id == feedbackId, trackChanges: changeTracking)
                .FirstOrDefaultAsync();
        }
    }
}
