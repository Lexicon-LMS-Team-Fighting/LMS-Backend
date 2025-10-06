using LMS.Shared.DTOs.UserDtos;

namespace Service.Contracts;

/// <summary>
/// Defines the contract for user-related business logic operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The user ID as a string, expected to be a valid GUID.</param>
    /// <returns>A <see cref="UserExtendedDto"/> representing the user.</returns>
    Task<UserExtendedDto> GetUserAsync(string userId);

	/// <summary>
	/// Retrieves all users from the data source.
	/// </summary>
	/// <returns>
	/// A collection of <see cref="UserDto"/> objects representing all users.
	/// </returns>
	Task<IEnumerable<UserWithRoleDto>> GetUsersAsync();
}
