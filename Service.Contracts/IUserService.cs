using LMS.Shared.DTOs.UserDtos;

namespace Service.Contracts;

/// <summary>
/// Defines the contract for user-related business logic operations. <br/>
/// Provides methods to retrieve user data as <see cref="UserDto"/> objects.
/// </summary>
public interface IUserService
{
	/// <summary>
	/// Retrieves a user by their unique identifier.
	/// </summary>
	/// <param name="userId">The user ID as a string, expected to be a valid GUID.</param>
	/// <returns>A <see cref="UserDto"/> representing the user.</returns>
	/// <exception cref="BadRequestException">
	/// Thrown when the provided <paramref name="userId"/> is not a valid GUID.
	/// </exception>
	/// <exception cref="UserNotFoundException">
	/// Thrown when no user is found with the given <paramref name="userId"/>.
	/// </exception>
	Task<UserDto> GetUserAsync(string userId);

	/// <summary>
	/// Retrieves all users from the data source.
	/// </summary>
	/// <returns>
	/// A collection of <see cref="UserDto"/> objects representing all users.
	/// </returns>
	Task<IEnumerable<UserDto>> GetUsersAsync();
}
