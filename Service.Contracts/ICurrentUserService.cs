namespace Service.Contracts
{
    /// <summary>
    /// Service to access information about the currently authenticated user.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Gets the unique identifier of the current user.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the username of the current user.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Gets the roles assigned to the current user.
        /// </summary>
        IReadOnlyCollection<string> Roles { get; }

        /// <summary>
        /// Indicates if the current user has the "Teacher" role. <br/>
        /// </summary>
        bool IsTeacher { get; }

        /// <summary>
        /// Indicates if the current user has the "Student" role. <br/>
        /// </summary>
        bool IsStudent { get; }
    }
}
