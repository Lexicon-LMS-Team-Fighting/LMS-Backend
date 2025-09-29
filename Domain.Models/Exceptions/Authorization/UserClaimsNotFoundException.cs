namespace Domain.Models.Exceptions.Authorization
{
    /// <summary>
    /// Exception type for scenarios where user claims are not found in the current context.
    /// </summary>
    public class UserClaimsNotFoundException : AuthorizationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserClaimsNotFoundException"/> class with a custom message.
        /// </summary>
        public UserClaimsNotFoundException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserClaimsNotFoundException"/> class with a default message.
        /// </summary>
        public UserClaimsNotFoundException()
            : base("Autorization failed: User claims not found.") { }
    }
}
