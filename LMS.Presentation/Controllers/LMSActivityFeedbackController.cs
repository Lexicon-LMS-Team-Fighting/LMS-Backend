using LMS.Shared.DTOs.LMSActivityFeedbackDtos;
using LMS.Shared.DTOs.ModuleDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.Presentation.Controllers
{
    /// <summary>
    /// Controller for managing feedbacks.
    /// Provides endpoints for common CRUD operations on feedback entities.
    /// </summary>
    [Route("api/feedbacks")]
    [ApiController]
    [Authorize]
    public class LMSActivityFeedbackController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="LMSActivityFeedbackController"/> class.
        /// </summary>
        /// <param name="serviceManager">The service manager for accessing feedback-related services.</param>
        public LMSActivityFeedbackController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Retrieves a specific feedback by its unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier of the feedback.</param>
        /// <returns>A <see cref="LMSActivityFeedbackExtendedDto"/> representing the feedback.</returns>
        /// <response code="200">Returns the feedback details.</response>
        /// <response code="404">If no feedback is found with the specified GUID.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpGet("{guid}")]
        [Authorize(Roles = "Teacher,Student")]
        [SwaggerOperation(
            Summary = "Get specified feedback by ID",
            Description = "Retrieves feedback details by their unique GUID identifier."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LMSActivityFeedbackExtendedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LMSActivityFeedbackExtendedDto>> GetFeedback(Guid guid) =>
            Ok(await _serviceManager.LMSActivityFeedbackService.GetByIdAsync(guid));

        /// <summary>
        /// Creates a new feedback.
        /// </summary>
        /// <param name="createDto">The details of the feedback to create.</param>
        /// <returns>The created feedback.</returns>
        /// <response code="201">Returns the created feedback.</response>
        /// <response code="400">If the provided feedback data is invalid.</response>
        /// <response code="409">If a feedback with the same identifier already exists.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpPost]
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
        public async Task<ActionResult<LMSActivityFeedbackExtendedDto>> CreateFeedback([FromBody] CreateLMSActivityFeedbackDto createDto)
        {
            var createdFeedback = await _serviceManager.LMSActivityFeedbackService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetFeedback), new { guid = createdFeedback.Id }, createdFeedback);
        }

        /// <summary>
        /// Updates an existing feedback.
        /// </summary>
        /// <param name="guid">The unique identifier of the feedback to update.</param>
        /// <param name="updateDto">The updated details of the feedback.</param>
        /// <response code="204">Feedback was successfully updated.</response>
        /// <response code="400">If the provided feedback data is invalid.</response>
        /// <response code="404">If no feedback is found with the specified GUID.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="409">If there is a conflict while updating the feedback.</response>
        [HttpPut("{guid}")]
        [Authorize(Roles = "Teacher")]
        [SwaggerOperation(
            Summary = "Update an existing feedback",
            Description = "Updates the details of an existing feedback identified by its GUID."
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateFeedback(Guid guid, [FromBody] UpdateLMSActivityFeedbackDto updateDto)
        {
            await _serviceManager.LMSActivityFeedbackService.UpdateAsync(guid, updateDto);
            return NoContent();
        }

        /// <summary>
        /// Deletes a feedback by its unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier of the feedback to delete.</param>
        /// <response code="204">Feedback was successfully deleted.</response>
        /// <response code="404">If no feedback is found with the specified GUID.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpDelete("{guid}")]
        [Authorize(Roles = "Teacher")]
        [SwaggerOperation(
            Summary = "Delete a feedback",
            Description = "Deletes the feedback identified by its GUID."
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteFeedback(Guid guid)
        {
            await _serviceManager.LMSActivityFeedbackService.DeleteAsync(guid);
            return NoContent();
        }
    }
}
