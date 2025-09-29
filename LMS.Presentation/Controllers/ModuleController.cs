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
        /// <returns>A <see cref="ModuleDto"/> representing the module.</returns>
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ModuleDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ModuleDto>> GetModule(Guid guid) =>
            Ok(await _serviceManager.ModuleService.GetByIdAsync(guid));

        /// <summary>
        /// Retrieves a paginated list of all modules.
        /// </summary>
        /// <param name="page">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResultDto<ModuleDto>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PaginatedResultDto<ModuleDto>>> GetModules([FromQuery] int page = 1, [FromQuery] int pageSize = 10) =>
            Ok(await _serviceManager.ModuleService.GetAllAsync(page, pageSize));

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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ModuleDto))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ModuleDto>> CreateModule([FromBody] CreateModuleDto module)
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
        /// <param name="page">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResultDto<LMSActivityDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<PaginatedResultDto<LMSActivityDto>>> GetActivitiesByModuleId(
            Guid moduleId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        ) =>
            Ok(await _serviceManager.LMSActivityService.GetAllByModuleIdAsync(moduleId, page, pageSize));

        /// <summary>
        /// Deletes a module by its unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier of the module to delete.</param>
        /// <response code="204">Module was successfully deleted.</response>
        /// <response code="404">If no module is found with the specified GUID.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        //[HttpDelete("{guid}")]
        //[Authorize(Roles = "Teacher")]
        //[SwaggerOperation(
        //    Summary = "Delete a module",
        //    Description = "Deletes the module identified by its GUID."
        //)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //public async Task<IActionResult> DeleteModule(Guid guid)
        //{
        //    await _serviceManager.ModuleService.DeleteAsync(guid);
        //    return NoContent();
        //}
    }
}
