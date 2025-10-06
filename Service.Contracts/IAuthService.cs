using LMS.Shared.DTOs.AuthDtos;
using LMS.Shared.DTOs.UserDtos;

namespace Service.Contracts;
public interface IAuthService
{
    Task<TokenDto> CreateTokenAsync(bool addTime);
    Task<TokenDto> RefreshTokenAsync(TokenDto token);
    Task<bool> ValidateUserAsync(UserAuthDto user);

    /// <summary>
    /// Creates a new user with the provided details.
    /// </summary>
    /// <param name="createDto">The details of the user to create.</param>
    /// <returns>The created user as a <see cref="UserExtendedDto"/>.</returns>
    Task<UserExtendedDto> CreateUserAsync(CreateUserDto createDto);

    /// <summary>
	/// Updates an existing user with the provided details.
	/// </summary>
	/// <param name="userId">The ID of the user to update.</param>
	/// <param name="updateDto">The updated user details.</param>
    Task UpdateUserAsync(string userId, UpdateUserDto updateDto);
}
