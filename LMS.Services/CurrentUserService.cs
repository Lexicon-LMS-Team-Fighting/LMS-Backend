using Domain.Models.Exceptions.Authorization;
using Microsoft.AspNetCore.Http;
using Service.Contracts;
using System.Security.Claims;

namespace LMS.Services;

/// <summary>
/// Service to retrieve information about the currently authenticated user.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    /// <summary>
    /// Gets the unique identifier of the current user.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the username of the current user.
    /// </summary>
    public string UserName { get; }

    /// <summary>
    /// Gets the roles assigned to the current user.
    /// </summary>
    public IReadOnlyCollection<string> Roles { get; }

    /// <summary>
    /// Indicates whether the current user has the "Teacher" role.
    /// </summary>
    public bool IsTeacher { get; }

    /// <summary>
    /// Indicate whether the current user has the "Student" role.
    /// </summary>
    public bool IsStudent { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrentUserService"/> class.
    /// Extracts user information from the HTTP context.
    /// </summary>
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var httpUser = httpContextAccessor.HttpContext?.User;

        Id = httpUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UserClaimsNotFoundException();

        UserName = httpUser?.FindFirst(ClaimTypes.Name)?.Value
            ?? throw new UserClaimsNotFoundException();

        Roles = httpUser?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList().AsReadOnly()
            ?? throw new UserClaimsNotFoundException();

        IsTeacher = Roles.Contains("Teacher");
        IsStudent = Roles.Contains("Student");
    }
}