using LMS.Shared.DTOs.LMSActivityDtos;
using LMS.Shared.DTOs.LMSActivityFeedbackDtos;
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
        /// Retrieves a specific feedback for a given activity and user.
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity.</param>
        /// <param name="userId">The unique identifier of the user to whom the feedback is provided.</param>
        /// <returns>A <see cref="LMSActivityFeedbackExtendedDto"/> representing the feedback.</returns>
        /// <response code="200">Returns the feedback details.</response>
        /// <response code="404">If no feedback is found for the specified activity and user.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpGet("activities/{activityId}/participants/{userId}/feedback")]
        [Authorize(Roles = "Teacher,Student")]
        [SwaggerOperation(
            Summary = "Get feedback by activity ID and user ID",
            Description = "Retrieves feedback details for a specific activity and user by their unique identifiers."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LMSActivityFeedbackExtendedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LMSActivityFeedbackExtendedDto>> GetFeedback(
            [FromRoute] Guid activityId,
            [FromRoute] string userId)
        {
            var feedback = await _serviceManager.LMSActivityFeedbackService.GetByActivityAndUserIdAsync(activityId, userId);
            if (feedback is null)
                return NotFound();

            return Ok(feedback);
        }

        /// <summary>
        /// Creates a new feedback.
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity.</param>
        /// <param name="userId">The unique identifier of the user to whom the feedback is provided.</param>
        /// <param name="createDto">The data for the feedback to create.</param>
        /// <returns>A <see cref="LMSActivityFeedbackExtendedDto"/> representing the created feedback.</returns>
        /// <response code="201">Returns the created feedback.</response>
        /// <response code="400">If the provided feedback data is invalid.</response>
        /// <response code="404">If the activity or user is not found.</response>
        /// <response code="409">If a feedback already exists for this activity and user.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpPost("{activityId}/participants/{userId}/feedback")]
        [Authorize(Roles = "Teacher")]
        [SwaggerOperation(
            Summary = "Create a new feedback",
            Description = "Creates a new feedback with the provided details."
        )]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LMSActivityFeedbackExtendedDto))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LMSActivityFeedbackExtendedDto>> CreateFeedback(
            [FromRoute] Guid activityId,
            [FromRoute] string userId,
            [FromBody] CreateLMSActivityFeedbackDto createDto)
        {
            var createdFeedback = await _serviceManager.LMSActivityFeedbackService.CreateAsync(activityId, userId, createDto);
            return CreatedAtAction(
                nameof(GetFeedback),
                new { activityId = createdFeedback.LMSActivityId, userId = createdFeedback.UserId },
                createdFeedback);
        }

        /// <summary>
        /// Updates an existing feedback.
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity.</param>
        /// <param name="userId">The unique identifier of the user to whom the feedback is provided.</param>
        /// <param name="updateDto">The updated data for the feedback.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the feedback was successfully updated.</response>
        /// <response code="400">If the provided feedback data is invalid.</response>
        /// <response code="404">If no feedback is found for the specified activity and user.</response>
        /// <response code="409">If there is a conflict during the update operation.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpPut("{activityId}/participants/{userId}/feedback")]
        [Authorize(Roles = "Teacher")]
        [SwaggerOperation(
            Summary = "Update an existing feedback",
            Description = "Updates the details of an existing feedback identified by activity ID and user ID."
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateFeedback(
            [FromRoute] Guid activityId,
            [FromRoute] string userId,
            [FromBody] UpdateLMSActivityFeedbackDto updateDto)
        {
            await _serviceManager.LMSActivityFeedbackService.UpdateAsync(activityId, userId, updateDto);
            return NoContent();
        }

        /// <summary>
        /// Deletes a feedback by activity ID and user ID.
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity.</param>
        /// <param name="userId">The unique identifier of the user to whom the feedback is provided.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">If the feedback was successfully deleted.</response>
        /// <response code="404">If no feedback is found for the specified activity and user.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpDelete("{activityId}/participants/{userId}/feedback")]
        [Authorize(Roles = "Teacher")]
        [SwaggerOperation(
            Summary = "Delete a feedback",
            Description = "Deletes the feedback identified by activity ID and user ID."
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteFeedback(
            [FromRoute] Guid activityId,
            [FromRoute] string userId)
        {
            await _serviceManager.LMSActivityFeedbackService.DeleteAsync(activityId, userId);
            return NoContent();
        }

        /// <summary>
        /// Deletes an activity by its unique identifier.
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">If the activity was successfully deleted.</response>
        /// <response code="404">If no activity is found with the specified ID.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpDelete("{activityId}")]
        [Authorize(Roles = "Teacher")]
        [SwaggerOperation(
            Summary = "Delete an activity",
            Description = "Deletes the LMS activity identified by its Id."
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteActivity(Guid activityId)
        {
            await _serviceManager.LMSActivityService.DeleteAsync(activityId);
            return NoContent();
        }
    }
}
