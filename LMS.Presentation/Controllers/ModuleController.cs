using LMS.Shared.DTOs.DocumentDtos;
using LMS.Shared.DTOs.LMSActivityDtos;
using LMS.Shared.DTOs.ModuleDtos;
using LMS.Shared.DTOs.PaginationDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.Presentation.Controllers
{
    /// <summary>
    /// Controller for managing modules.
    /// Provides endpoints for creating, retrieving, updating, and deleting modules,
    /// as well as retrieving modules by course and paginated results.
    /// </summary>
    [Route("api/modules")]
    [ApiController]
    [Authorize]
    public class ModuleController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleController"/> class.
        /// </summary>
        /// <param name="serviceManager">The service manager for accessing module-related services.</param>
        public ModuleController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Retrieves a specific module by its unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier of the module.</param>
        /// <param name="include">Related entities to include (e.g., "lmsactivities", "participants", "documents").</param>
        /// <returns>A <see cref="ModuleExtendedDto"/> representing the module.</returns>
        /// <response code="200">Returns the module details.</response>
        /// <response code="404">If no module is found with the specified GUID.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpGet("{guid}")]
        [Authorize(Roles = "Teacher,Student")]
        [SwaggerOperation(
            Summary = "Get specified module by ID",
            Description = "Retrieves module details by their unique GUID identifier."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ModuleExtendedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ModuleExtendedDto>> GetModule(Guid guid, [FromQuery] string? include) =>
            Ok(await _serviceManager.ModuleService.GetByIdAsync(guid, include));

        /// <summary>
        /// Retrieves a paginated list of all modules.
        /// </summary>
        /// <param name="query">Pagination and filtering parameters.</param>
        /// <returns>A paginated list of modules.</returns>
        /// <response code="200">Returns a paginated list of modules.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpGet]
        [Authorize(Roles = "Teacher,Student")]
        [SwaggerOperation(
            Summary = "Get all modules",
            Description = "Retrieves a list of all modules in the system."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResultDto<ModulePreviewDto>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PaginatedResultDto<ModulePreviewDto>>> GetModules([FromQuery] PaginatedQueryDto query) =>
            Ok(await _serviceManager.ModuleService.GetAllAsync(query));

        /// <summary>
        /// Creates a new module.
        /// </summary>
        /// <param name="module">The details of the module to create.</param>
        /// <returns>The created module.</returns>
        /// <response code="201">Returns the created module.</response>
        /// <response code="400">If the provided module data is invalid.</response>
        /// <response code="409">If a module with the same identifier already exists.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [SwaggerOperation(
            Summary = "Create a new module",
            Description = "Creates a new module with the provided details."
        )]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ModuleExtendedDto))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ModuleExtendedDto>> CreateModule([FromBody] CreateModuleDto module)
        {
            var createdModule = await _serviceManager.ModuleService.CreateAsync(module);
            return CreatedAtAction(nameof(GetModule), new { guid = createdModule.Id }, createdModule);
        }

        /// <summary>
        /// Updates an existing module.
        /// </summary>
        /// <param name="guid">The unique identifier of the module to update.</param>
        /// <param name="module">The updated details of the module.</param>
        /// <response code="204">Module was successfully updated.</response>
        /// <response code="400">If the provided module data is invalid.</response>
        /// <response code="404">If no module is found with the specified GUID.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="409">If there is a conflict while updating the module.</response>
        [HttpPut("{guid}")]
        [Authorize(Roles = "Teacher")]
        [SwaggerOperation(
            Summary = "Update an existing module",
            Description = "Updates the details of an existing module identified by its GUID."
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateModule(Guid guid, [FromBody] UpdateModuleDto module)
        {
            await _serviceManager.ModuleService.UpdateAsync(guid, module);
            return NoContent();
        }

        /// <summary>
        /// Retrieves a paginated list of activities for a specific module.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="query">Pagination and filtering parameters.</param>
        /// <returns>A paginated list of activities for the specified module.</returns>
        /// <response code="200">Returns the paginated list of activities.</response>
        /// <response code="400">If the provided GUID is not valid.</response>
        /// <response code="404">If no module is found with the specified GUID.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpGet("{moduleId}/activities")]
        [Authorize(Roles = "Teacher,Student")]
        [SwaggerOperation(
            Summary = "Get all activities for a specific module",
            Description = "Retrieves all activities associated with the specified module ID."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResultDto<LMSActivityPreviewDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<PaginatedResultDto<LMSActivityPreviewDto>>> GetActivitiesByModuleId(
            Guid moduleId,
            [FromQuery] PaginatedQueryDto query
        ) =>
            Ok(await _serviceManager.LMSActivityService.GetAllByModuleIdAsync(moduleId, query));

        /// <summary>
        /// Deletes a module by its unique identifier.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module to delete.</param>
        /// <response code="204">Module was successfully deleted.</response>
        /// <response code="404">If no module is found with the specified GUID.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpDelete("{moduleId}")]
        [Authorize(Roles = "Teacher")]
        [SwaggerOperation(
            Summary = "Delete a module",
            Description = "Deletes the module identified by its Id."
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteModule(Guid moduleId)
        {
            await _serviceManager.ModuleService.DeleteAsync(moduleId);
            return NoContent();
        }

        /// <summary>
        /// Retrieves paginated documents attached to a specific module.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="page">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
        /// <response code="200">Returns a paginated list of documents for the specified module.</response>
        /// <response code="404">If the module is not found.</response>
        [HttpGet("modules/{moduleId}/documents")]
        [Authorize]
        [SwaggerOperation(
            Summary = "Get paginated documents for a module",
            Description = "Returns paginated documents attached to the specified module."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResultDto<DocumentPreviewDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetDocumentsByModule(
            Guid moduleId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var documents = await _serviceManager.DocumentService.GetAllByModuleIdAsync(moduleId, page, pageSize);
            return Ok(documents);
        }


        /// <summary>
        /// Attaches an existing document to a module.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="documentId">The unique identifier of the document to attach.</param>
        /// <response code="204">Document was successfully attached.</response>
        /// <response code="404">If no module or document is found with the specified GUID.</response>
        /// <response code="409">If the document is already attached to this module.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpPost("modules/{moduleId}/documents/{documentId}")]
        [Authorize(Roles = "Student,Teacher")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> AttachDocumentToModule(Guid moduleId, Guid documentId)
        {
            await _serviceManager.DocumentService.AttachToModuleAsync(moduleId, documentId);
            return NoContent();
        }

        /// <summary>
        /// Removes a document from a module.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="documentId">The unique identifier of the document to remove.</param>
        /// <response code="204">Document was successfully detached.</response>
        /// <response code="404">If no module or document is found with the specified GUID.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpDelete("modules/{moduleId}/documents/{documentId}")]
        [Authorize(Roles = "Student,Teacher")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DetachDocumentFromModule(Guid moduleId, Guid documentId)
        {
            await _serviceManager.DocumentService.DetachFromModuleAsync(moduleId, documentId);
            return NoContent();
        }
    }
}
