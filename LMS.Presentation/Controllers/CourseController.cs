using LMS.Shared.DTOs.CourseDtos;
using LMS.Shared.DTOs.LMSActivityDtos;
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
[Authorize]
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
	/// <remarks>Requires authentication as either <c>Teacher</c> or <c>Student</c>.</remarks>
	/// <returns>
	/// An <see cref="ActionResult{T}"/> containing the <see cref="CourseDto"/> 
	/// if found, or an appropriate error response.
	/// </returns>
	/// <response code="200">Returns the user details.</response>
	/// <response code="400">If the provided GUID is not valid.</response>
	/// <response code="401">The request is unauthorized (missing or invalid token).</response>
	/// <response code="403">The authenticated user does not have the required role.</response>
	/// <response code="404">If no course is found with the specified GUID.</response>
	[HttpGet("{guid}")]
	[Authorize(Roles = "Teacher,Student")]
	[SwaggerOperation(
		Summary = "Get specified course by ID",
		Description = "Retrieves course details by their unique GUID identifier."
	)]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CourseDetailedDto>))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<CourseDetailedDto>> GetCourse(Guid guid) =>
		Ok(await _serviceManager.CourseService.GetCourseAsync(guid));


	/// <summary>Retrieves all courses.</summary>
	/// <remarks>Requires authentication as either <c>Teacher</c> or <c>Student</c>.</remarks>
	/// <returns>An <see cref="ActionResult{T}"/> containing a collection of <see cref="CourseDto"/> objects.</returns>
	/// <response code="200">Returns the list of users (empty if none exist).</response>
	/// <response code="401">The request is unauthorized (missing or invalid token).</response>
	/// <response code="403">The authenticated user does not have the required role.</response>
	[HttpGet]
	[Authorize(Roles = "Teacher,Student")]
	[SwaggerOperation(
		Summary = "Get all Courses",
		Description = "Retrieves a list of all Courses in the system."
	)]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CourseDto>))]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<ActionResult<IEnumerable<UserDto>>> GetCourses() =>
		Ok(await _serviceManager.CourseService.GetCoursesAsync());

    /// <summary>Retrieves a paginated list of modules for a specific course.</summary>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <param name="page">The page number to retrieve (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <returns>A paginated list of modules for the specified course.</returns>
    /// <response code="200">Returns the paginated list of modules.</response>
    /// <response code="400">If the provided GUID is not valid.</response>
    /// <response code="404">If no course is found with the specified GUID.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet("{courseId}/modules")]
    [Authorize(Roles = "Teacher,Student")]
    [SwaggerOperation(
        Summary = "Get all modules for a specific course",
        Description = "Retrieves all modules associated with the specified course ID."
    )]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResultDto<ModuleDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<PaginatedResultDto<ModuleDto>>> GetModulesByCourseId(
        Guid courseId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    ) =>
        Ok(await _serviceManager.ModuleService.GetAllByCourseIdAsync(courseId, page, pageSize));

	/// <summary>
	/// Creates a course.
	/// </summary>
	/// <remarks> Must be authorized as a <c>Teacher</c> to access this endpoint.</remarks>
	/// <returns> A <see cref="CourseDto"/> representing the created course, including a link to retrieve it.</returns>
	/// <param name="createCourseDto">The data required to create the course.</param>
	/// <response code="201">Returns A link to <see cref="CourseDto"/>.</response>
	/// <response code="400">The end data is earlier then or equal to start date.</response>
	/// <response code="401">Unauthorized.</response>
	/// <response code="403">Forbidden.</response>
	/// <response code="409">Course with that name already exists.</response>
	[HttpPost]
	[Authorize(Roles ="Teacher")]
	[SwaggerOperation(
		Summary = "Create a new course",
		Description = "Creates a new course with the provided details. Requires Teacher role."
	)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
	public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] CreateCourseDto createCourseDto)
	{
		var createdCourse = await _serviceManager.CourseService.CreateCourseAsync(createCourseDto);
		return CreatedAtAction(nameof(GetCourse), new { Guid = createdCourse.Id }, createdCourse);
	}
}
