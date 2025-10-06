namespace Domain.Models.Exceptions.Authorization
{
    /// <summary>
    /// Exception type for scenarios where user role is not found
    /// </summary>
    public class UserRoleNotFoundException : AuthorizationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserClaimsNotFoundException"/> class with a default message.
        /// </summary>
        public UserRoleNotFoundException()
            : base("Autorization failed: User role not found.") { }
    }
}
