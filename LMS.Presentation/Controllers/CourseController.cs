using LMS.Shared.DTOs.CourseDtos;
using LMS.Shared.DTOs.ModuleDtos;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.DTOs.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.Presentation.Controllers;

/// <summary>Provides API endpoints for managing course within the LMS system.</summary>
/// <remarks>
/// The controller uses the <see cref="IServiceManager"/> abstraction to delegate 
/// business logic and data access to the service layer.
/// </remarks>
[Route("api/course")]
[ApiController]
public class CourseController: ControllerBase
{
	public IServiceManager _serviceManager;

	public CourseController(IServiceManager serviceManager)
	{
		_serviceManager = serviceManager;
	}

	/// <summary>
	/// Retrieves a specific user by their unique identifier.
	/// </summary>
	/// <param name="guid">The unique identifier of the course (GUID format).</param>
	/// <returns>
	/// An <see cref="ActionResult{T}"/> containing the <see cref="CourseDto"/> 
	/// if found, or an appropriate error response.
	/// </returns>
	/// <response code="200">Returns the user details.</response>
	/// <response code="400">If the provided GUID is not valid.</response>
	/// <response code="404">If no course is found with the specified GUID.</response>
	[HttpGet("{guid}")]
	[SwaggerOperation(
		Summary = "Get specified course by ID",
		Description = "Retrieves course details by their unique GUID identifier."
	)]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CourseDto>))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<CourseDto>> GetCourse(Guid guid) =>
		Ok(await _serviceManager.CourseService.GetCourseAsync(guid));



	/// <summary>Retrieves all courses.</summary>
	/// <returns>
	/// An <see cref="ActionResult{T}"/> containing a collection of <see cref="CourseDto"/> objects.
	/// </returns>
	/// <response code="200">Returns the list of users (empty if none exist).</response>
	[HttpGet]
	[SwaggerOperation(
		Summary = "Get all Courses",
		Description = "Retrieves a list of all Courses in the system."
	)]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CourseDto>))]
	public async Task<ActionResult<IEnumerable<UserDto>>> GetCourses() =>
		Ok(await _serviceManager.CourseService.GetCoursesAsync());

    /// <summary>
    /// Retrieves a paginated list of modules for a specific course.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <param name="page">The page number to retrieve (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <returns>A paginated list of modules for the specified course.</returns>
    [HttpGet("{courseId}/modules")]
    [Authorize(Roles = "Teacher,Student")]
    [SwaggerOperation(
        Summary = "Get all modules for a specific course",
        Description = "Retrieves all modules associated with the specified course ID."
    )]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResultDto<ModuleDto>))]
	[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PaginatedResultDto<ModuleDto>>> GetModulesByCourseId(Guid courseId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10) =>
        Ok(await _serviceManager.ModuleService.GetAllByCourseIdAsync(courseId, page, pageSize));
}
