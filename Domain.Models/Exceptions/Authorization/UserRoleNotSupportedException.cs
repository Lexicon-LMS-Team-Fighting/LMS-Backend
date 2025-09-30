namespace Domain.Models.Exceptions.Authorization
{
    /// <summary>
    /// Exception type for cases where a user's role is not supported for a specific operation.
    /// </summary>
    public class UserRoleNotSupportedException : AuthorizationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleNotSupportedException"/> class with a custom message.
        /// </summary>
        public UserRoleNotSupportedException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleNotSupportedException"/> class with a default message.
        /// </summary>
        public UserRoleNotSupportedException()
            : base("The user role is not supported for this operation.") { }
    }
}
