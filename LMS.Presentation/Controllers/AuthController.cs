using LMS.Shared.DTOs.AuthDtos;
using LMS.Shared.DTOs.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.Presentation.Controllers;

/// <summary>
/// Provides API endpoints for authentication and user management within the LMS system.
/// </summary>
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="serviceManager">The service manager for accessing authentication and user-related services.</param>
    public AuthController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token upon successful validation of credentials.
    /// </summary>
    /// <param name="user">The user credentials for authentication.</param>
    /// <returns>A JWT token if authentication is successful; otherwise, an unauthorized response.</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Authenticate user",
        Description = "Validates user credentials and returns a JWT token for authorization."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Authentication successful", typeof(TokenDto))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid username or password")]
    public async Task<IActionResult> Authenticate(UserAuthDto user)
    {
        if (!await _serviceManager.AuthService.ValidateUserAsync(user))
            return Unauthorized();

        var tokenDto = await _serviceManager.AuthService.CreateTokenAsync(addTime: true);
        return Ok(tokenDto);
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="createDto">The details of the user to create.</param>
    /// <returns>The created user.</returns>
    /// <response code="201">Returns the created user.</response>
    /// <response code="400">If the provided user data is invalid.</response>
    /// <response code="409">If a user with the same data already exists.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpPost]
    [Authorize(Roles = "Teacher")]
    [SwaggerOperation(
        Summary = "Create a new user",
        Description = "Creates a new user with the provided details."
    )]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserExtendedDto))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserExtendedDto>> CreateUser([FromBody] CreateUserDto createDto)
    {
        var createdUser = await _serviceManager.AuthService.CreateUserAsync(createDto);
        return Created(string.Empty, createdUser);
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="guid">The unique identifier of the user to update.</param>
    /// <param name="userDto">The updated details of the user.</param>
    /// <response code="204">User was successfully updated.</response>
    /// <response code="400">If the provided user data is invalid.</response>
    /// <response code="404">If no user is found with the specified GUID.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    /// <response code="409">If there is a conflict while updating the user.</response>
    [HttpPut("{userId}")]
    [Authorize(Roles = "Teacher")]
    [SwaggerOperation(
        Summary = "Update an existing user",
        Description = "Updates the details of an existing user identified by its GUID."
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDto userDto)
    {
        await _serviceManager.AuthService.UpdateUserAsync(userId, userDto);
        return NoContent();
    }
}
