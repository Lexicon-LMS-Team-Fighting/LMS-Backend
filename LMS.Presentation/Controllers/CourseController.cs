using LMS.Shared.DTOs.CourseDtos;
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
    /// <param name="include">Related entities to include (e.g., "participants", "modules", "documents").</param>
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
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CourseExtendedDto>))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<CourseExtendedDto>> GetCourse(Guid guid, [FromQuery] string? include) =>
		Ok(await _serviceManager.CourseService.GetCourseAsync(guid, include));


    /// <summary>Retrieves all courses.</summary>
    /// <remarks>Requires authentication as either <c>Teacher</c> or <c>Student</c>.</remarks>
    /// <param name="pageNumber">The page number to retrieve (default is 1).</param>
	/// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="include">Optional fields to include (e.g., "progress").</param>
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
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResultDto<CoursePreviewDto>))]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<ActionResult<PaginatedResultDto<CoursePreviewDto>>> GetCourses(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? include = null) =>
		Ok(await _serviceManager.CourseService.GetCoursesAsync(pageNumber, pageSize, include));

    /// <summary>Retrieves a paginated list of modules for a specific course.</summary>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <param name="page">The page number to retrieve (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="include">Optional fields to include.</param>
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResultDto<ModulePreviewDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<PaginatedResultDto<ModulePreviewDto>>> GetModulesByCourseId(
        Guid courseId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? include = null
    ) =>
        Ok(await _serviceManager.ModuleService.GetAllByCourseIdAsync(courseId, page, pageSize, include));

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
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseExtendedDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
	public async Task<ActionResult<CourseExtendedDto>> CreateCourse([FromBody] CreateCourseDto createCourseDto)
	{
		var createdCourse = await _serviceManager.CourseService.CreateAsync(createCourseDto);
		return CreatedAtAction(nameof(GetCourse), new { Guid = createdCourse.Id }, createdCourse);
	}

    /// <summary>
    /// Retrieves the participants of a course.
    /// </summary>
    /// <remarks> Must be authorized as a <c>Student</c> or <c>Teacher</c> to access this endpoint.</remarks>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <param name="pageNumber">The page number to retrieve (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <returns>A paginated list of <see cref="PaginatedResultDto{CourseParticipantDto}"/> representing the participants of the course.</returns>
    /// <response code="200">Returns the list of participants.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    /// <response code="404">Course not found.</response>
    [HttpGet("{courseId:guid}/participants")]
    [Authorize(Roles = "Student,Teacher")]
    [SwaggerOperation(
        Summary = "Get course participants",
        Description = "Retrieves all participants of a specific course. Requires Student or Teacher role."
    )]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResultDto<CourseParticipantDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaginatedResultDto<CourseParticipantDto>>> GetCourseParticipants(Guid courseId, int pageNumber = 1, int pageSize = 10) =>
        Ok(await _serviceManager.CourseService.GetParticipantsAsync(courseId, pageNumber, pageSize));

    /// Enrolls a student into a specific course.
    /// </summary>
    /// <remarks>Must be authorized as a <c>Teacher</c> to access this endpoint.</remarks>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <param name="studentId">The unique identifier of the student to enroll.</param>
    /// <response code="204">Student was successfully enrolled in the course.</response>
    /// <response code="400">Invalid request (e.g., student already enrolled).</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    /// <response code="404">Course or student not found.</response>
    [HttpPost("{courseId:guid}/participants/{studentId:guid}")]
    [Authorize(Roles = "Teacher")]
    [SwaggerOperation(
        Summary = "Enroll a student into a course",
        Description = "Adds the specified student to the participants list of the course. Requires Teacher role."
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> EnrollStudent(Guid courseId, Guid studentId)
    {
        await _serviceManager.CourseService.EnrollStudentAsync(courseId, studentId.ToString());
        return NoContent();
    }

    /// <summary>
    /// Unenrolls a student from a specific course.
    /// </summary>
    /// <remarks>Must be authorized as a <c>Teacher</c> to access this endpoint. 
    /// When a student is removed, all their submitted feedback for this course will also be deleted.</remarks>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <param name="studentId">The unique identifier of the student to unenroll.</param>
    /// <response code="204">Student was successfully unenrolled from the course.</response>
    /// <response code="400">Invalid request (e.g., student not enrolled).</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    /// <response code="404">Course or student not found.</response>
    [HttpDelete("{courseId:guid}/participants/{studentId:guid}")]
    [Authorize(Roles = "Teacher")]
    [SwaggerOperation(
        Summary = "Unenroll a student from a course",
        Description = "Removes the specified student from the participants list of the course and deletes all their feedback related to the course. Requires Teacher role."
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> UnenrollStudent(Guid courseId, Guid studentId)
    {
        await _serviceManager.CourseService.UnenrollStudentAsync(courseId, studentId.ToString());
        return NoContent();
    }

    /// <summary>
    /// Updates an existing course.
    /// </summary>
    /// <param name="guid">The unique identifier of the course to update.</param>
    /// <param name="updateDto">The updated details of the course.</param>
    /// <response code="204">Course was successfully updated.</response>
    /// <response code="400">If the provided course data is invalid.</response>
    /// <response code="404">If no course is found with the specified GUID.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    /// <response code="409">If there is a conflict while updating the course.</response>
    [HttpPut("{guid}")]
    [Authorize(Roles = "Teacher")]
    [SwaggerOperation(
        Summary = "Update an existing course",
        Description = "Updates the details of an existing course identified by its GUID."
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateCourse(Guid guid, [FromBody] UpdateCourseDto updateDto)
    {
        await _serviceManager.CourseService.UpdateAsync(guid, updateDto);
        return NoContent();
    }

    /// <summary>
    /// Deletes a course by its unique identifier.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course to delete.</param>
    /// <response code="204">Course was successfully deleted.</response>
    /// <response code="404">If no course is found with the specified GUID.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpDelete("{courseId}")]
    [Authorize(Roles = "Teacher")]
    [SwaggerOperation(
        Summary = "Delete a course",
        Description = "Deletes the course identified by its Id."
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteCourse(Guid courseId)
    {
        await _serviceManager.CourseService.DeleteAsync(courseId);
        return NoContent();
    }

    /// <summary>
    /// Retrieves paginated documents attached to a specific course.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <param name="page">The page number to retrieve (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <response code="200">Returns a paginated list of documents for the specified course.</response>
    /// <response code="404">If the course is not found.</response>
    [HttpGet("courses/{courseId}/documents")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Get paginated documents for a course",
        Description = "Returns paginated documents attached to the specified course."
    )]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResultDto<DocumentPreviewDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetDocumentsByCourse(
        Guid courseId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var documents = await _serviceManager.DocumentService.GetAllByCourseIdAsync(courseId, page, pageSize);
        return Ok(documents);
    }


    /// <summary>
    /// Attaches an existing document to a course.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <param name="documentId">The unique identifier of the document to attach.</param>
    /// <response code="204">Document was successfully attached.</response>
    /// <response code="404">If no course or document is found with the specified GUID.</response>
    /// <response code="409">If the document is already attached to this course.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpPost("{courseId}/documents/{documentId}")]
    [Authorize(Roles = "Teacher")]
    [SwaggerOperation(
        Summary = "Attach a document to a course",
        Description = "Attaches an existing document to the specified course."
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AttachDocumentToCourse(Guid courseId, Guid documentId)
    {
        await _serviceManager.DocumentService.AttachToCourseAsync(courseId, documentId);
        return NoContent();
    }

    /// <summary>
    /// Removes a document from a course.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <param name="documentId">The unique identifier of the document to remove.</param>
    /// <response code="204">Document was successfully detached.</response>
    /// <response code="404">If no course or document is found with the specified GUID.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpDelete("{courseId}/documents/{documentId}")]
    [Authorize(Roles = "Teacher")]
    [SwaggerOperation(
        Summary = "Detach a document from a course",
        Description = "Removes an existing document from the specified course."
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DetachDocumentFromCourse(Guid courseId, Guid documentId)
    {
        await _serviceManager.DocumentService.DetachFromCourseAsync(courseId, documentId);
        return NoContent();
    }

}