using Domain.Models.Entities;

namespace Domain.Contracts.Repositories
{
    /// <summary>
    /// Repository contract for managing <see cref="UserCourse"/> entities.
    /// Inherits basic CRUD operations from <see cref="IRepositoryBase{T}"/>.
    /// Provides methods to retrieve, add, update, and delete enrollments
    /// </summary>
    public interface IUserCourseRepository : IRepositoryBase<UserCourse>
    {
        /// <summary>
        /// Deletes all <see cref="UserCourse"/> entries associated with a specific user.
        /// </summary>
        /// <param name="studentId">The unique identifier of the user whose enrollments are being deleted.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        Task DeleteAllByUserId(string studentId);
    }
}
