using LMS.Shared.DTOs.LMSActivityDtos;
using LMS.Shared.DTOs.PaginationDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.Presentation.Controllers
{
    /// <summary>
    /// Controller for managing LMS activities.
    /// Provides endpoints for creating, retrieving, updating, and deleting activities,
    /// as well as retrieving paginated results.
    /// </summary>
    [Route("api/lms-activities")]
    [ApiController]
    [Authorize]
    public class LMSActivitiesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="LMSActivitiesController"/> class.
        /// </summary>
        /// <param name="serviceManager">The service manager for accessing activity-related services.</param>
        public LMSActivitiesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Retrieves a specific activity by its unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier of the activity.</param>
        /// <param name="include">Related entities to include (e.g., "participants", "feedbacks", "documents").</param>
        /// <returns>A <see cref="LMSActivityExtendedDto"/> representing the activity.</returns>
        /// <response code="200">Returns the activity details.</response>
        /// <response code="404">If no activity is found with the specified GUID.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpGet("{guid}")]
        [Authorize(Roles = "Teacher,Student")]
        [SwaggerOperation(
            Summary = "Get specified activity by ID",
            Description = "Retrieves activity details by their unique GUID identifier."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LMSActivityExtendedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LMSActivityExtendedDto>> GetActivity(Guid guid, [FromQuery] string? include) =>
            Ok(await _serviceManager.LMSActivityService.GetByIdAsync(guid, include));

        /// <summary>
        /// Retrieves a paginated list of all activities.
        /// </summary>
        /// <param name="page">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
        /// <returns>A paginated list of activities.</returns>
        /// <response code="200">Returns a paginated list of activities.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpGet]
        [Authorize(Roles = "Teacher,Student")]
        [SwaggerOperation(
            Summary = "Get all activities",
            Description = "Retrieves a list of all activities in the system."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResultDto<LMSActivityPreviewDto>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PaginatedResultDto<LMSActivityPreviewDto>>> GetActivities([FromQuery] int page = 1, [FromQuery] int pageSize = 10) =>
            Ok(await _serviceManager.LMSActivityService.GetAllAsync(page, pageSize));

        /// <summary>
        /// Creates a new activity.
        /// </summary>
        /// <param name="activity">The details of the activity to create.</param>
        /// <returns>The created activity.</returns>
        /// <response code="201">Returns the created activity.</response>
        /// <response code="400">If the provided activity data is invalid.</response>
        /// <response code="409">If an activity with the same identifier already exists.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [SwaggerOperation(
            Summary = "Create a new activity",
            Description = "Creates a new LMS activity with the provided details."
        )]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LMSActivityExtendedDto))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LMSActivityExtendedDto>> CreateActivity([FromBody] CreateLMSActivityDto activity)
        {
            var createdActivity = await _serviceManager.LMSActivityService.CreateAsync(activity);
            return CreatedAtAction(nameof(GetActivity), new { guid = createdActivity.Id }, createdActivity);
        }

        /// <summary>
        /// Updates an existing activity.
        /// </summary>
        /// <param name="guid">The unique identifier of the activity to update.</param>
        /// <param name="activity">The updated details of the activity.</param>
        /// <response code="204">Activity was successfully updated.</response>
        /// <response code="400">If the provided activity data is invalid.</response>
        /// <response code="404">If no activity is found with the specified GUID.</response>
        /// <response code="409">If there is a conflict while updating the activity.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpPut("{guid}")]
        [Authorize(Roles = "Teacher")]
        [SwaggerOperation(
            Summary = "Update an existing activity",
            Description = "Updates the details of an existing activity identified by its GUID."
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateActivity(Guid guid, [FromBody] UpdateLMSActivityDto activity)
        {
            await _serviceManager.LMSActivityService.UpdateAsync(guid, activity);
            return NoContent();
        }

        /// <summary>
        /// Deletes an activity by its unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier of the activity to delete.</param>
        /// <response code="204">Activity was successfully deleted.</response>
        /// <response code="404">If no activity is found with the specified GUID.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpDelete("{guid}")]
        [Authorize(Roles = "Teacher")]
        [SwaggerOperation(
            Summary = "Delete an activity",
            Description = "Deletes the LMS activity identified by its GUID."
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteActivity(Guid guid)
        {
            await _serviceManager.LMSActivityService.DeleteAsync(guid);
            return NoContent();
        }
    }
}
