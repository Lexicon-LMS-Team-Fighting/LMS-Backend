using LMS.Shared.DTOs.DocumentDtos;
using LMS.Shared.DTOs.ModuleDtos;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.DTOs.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.Presentation.Controllers;

/// <summary>Provides API endpoints for managing users within the LMS system.</summary>
/// <remarks>
/// The controller uses the <see cref="IServiceManager"/> abstraction to delegate 
/// business logic and data access to the service layer.
/// </remarks>
[Route("api/user")]
[ApiController]
[Authorize]
public class UserController: ControllerBase
{
	public IServiceManager _serviceManager;

	/// <summary>
	/// Initializes a new instance of the <see cref="UserController"/> class.
	/// </summary>
	/// <param name="serviceManager">The service manager for accessing module-related services.</param>
	public UserController(IServiceManager serviceManager)
	{
		_serviceManager = serviceManager;
	}

	/// <summary>
	/// Retrieves a specific user by their unique identifier.
	/// </summary>
	/// <remarks>Requires authentication as either <c>Teacher</c> or <c>Student</c>.</remarks>
	/// <param name="guid">The unique identifier of the user as a string (GUID format).</param>
	/// <returns>
	/// An <see cref="ActionResult{T}"/> containing the <see cref="UserDto"/> 
	/// if found, or an appropriate error response.
	/// </returns>
	/// <response code="200">Returns the user details.</response>
	/// <response code="400">If the provided GUID is not valid.</response>
	/// <response code="401">The request is unauthorized (missing or invalid token).</response>
	/// <response code="403">The authenticated user does not have the required role.</response>
	/// <response code="404">If no user is found with the specified GUID.</response>
	[HttpGet("{guid}")]
	[Authorize(Roles = "Teacher,Student")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserExtendedDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[SwaggerOperation(
		Summary = "Get specified user by ID",
		Description = "Retrieves user details by their unique GUID identifier."
	)]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserExtendedDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
	[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
	public async Task<ActionResult<UserExtendedDto>> GetUser(string guid) => 
		Ok(await _serviceManager.UserService.GetUserAsync(guid));

    /// <summary>Retrieves all users.</summary>
    /// <returns>
    /// An <see cref="ActionResult{T}"/> containing a collection of <see cref="UserWithRolesDto"/> objects.
    /// </returns>
    /// <remarks>Requires authentication as either <c>Teacher</c> or <c>Student</c>.</remarks>
    /// <response code="200">Returns the list of users (empty if none exist).</response>
    /// <response code="401">The request is unauthorized (missing or invalid token).</response>
    /// <response code="403">The authenticated user does not have the required role.</response>
    [HttpGet]
	[Authorize(Roles = "Teacher")]
	[SwaggerOperation(
		Summary = "Get all users",
		Description = "Retrieves a list of all users in the system."
	)]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserWithRolesDto>))]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<ActionResult<IEnumerable<UserWithRolesDto>>> GetUsers() => 
		Ok(await _serviceManager.UserService.GetUsersAsync());


    /// <summary>
	/// Retrieves a paginated list of documents available to the specified user.
	/// </summary>
	/// <remarks>This endpoint is restricted to users with the "Teacher" role.  If the user is not authorized, a 401
	/// Unauthorized response is returned.  If the user does not have the required role, a 403 Forbidden response is
	/// returned.</remarks>
	/// <param name="userId">The unique identifier of the user whose documents are being retrieved.</param>
	/// <param name="page">The page number of the results to retrieve. Defaults to <see langword="1"/>.</param>
	/// <param name="pageSize">The number of items per page. Defaults to <see langword="10"/>.</param>
	/// <returns>A <see cref="PaginatedResultDto{T}"/> containing a paginated list of <see cref="DocumentPreviewDto"/> objects.
	/// Returns a 200 OK response if successful.</returns>
	/// <response code="200">Returns the list of users (empty if none exist).</response>
    /// <response code="401">The request is unauthorized (missing or invalid token).</response>
    /// <response code="403">The authenticated user does not have the required role.</response>
    [HttpGet("{userId}/documents")]
    [Authorize(Roles = "Teacher")]
    [SwaggerOperation(
        Summary = "Get all documents (paginated)",
        Description = "Retrieves a paginated list of all documents available to the user."
    )]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResultDto<DocumentPreviewDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PaginatedResultDto<DocumentPreviewDto>>> GetDocuments(
		[FromRoute] string userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var documents = await _serviceManager.DocumentService.GetAllByUserIdAsync(userId, page, pageSize);
        return Ok(documents);
    }
}
