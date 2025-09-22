using LMS.Shared.DTOs.UserDtos;

namespace Service.Contracts;

/// <summary>
/// Defines the contract for user-related business logic operations. <br/>
/// Provides methods to retrieve user data as <see cref="UserDto"/> objects.
/// </summary>
public interface IUserService
{
	/// <summary>
	/// Retrieves a single user by its unique identifier. <br/>
	/// This method returns a <see cref="UserDto"/>, 
	/// </summary>
	/// <param name="userId">The unique identifier of the user, typically a string (e.g., GUID as string).</param>
	/// <returns>
	/// A task that represents the asynchronous operation. 
	/// The task result contains the corresponding <see cref="UserDto"/> if found, 
	/// otherwise it may throw an exception or return <c>null</c> depending on the implementation.
	/// </returns>
	Task<UserDto> GetUserAsync(string userId);

	/// <summary>
	/// Retrieves all users. <br/>
	/// Returns a collection of <see cref="UserDto"/> objects for use in client-facing layers.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation. 
	/// The task result contains an enumerable collection of <see cref="UserDto"/> objects.
	/// </returns>
	Task<IEnumerable<UserDto>> GetUsersAsync();
}
