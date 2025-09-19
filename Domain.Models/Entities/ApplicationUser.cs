using Microsoft.AspNetCore.Identity;

namespace Domain.Models.Entities;

/// <summary>
/// Represents an application user with authentication and authorization details.
/// Inherits from <see cref="IdentityUser"/> to leverage ASP.NET Core Identity features.
/// </summary>
public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpireTime { get; set; }

    public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
    public ICollection<Document> Documents { get; set; } = new List<Document>();
}