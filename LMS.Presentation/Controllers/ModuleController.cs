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
        [HttpGet("{guid}")]
        [Authorize(Roles = "Teacher,Student")]
        [SwaggerOperation(
            Summary = "Get specified module by ID",
            Description = "Retrieves module details by their unique GUID identifier."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ModuleDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ModuleDto>> GetModule(Guid guid) =>
            Ok(await _serviceManager.ModuleService.GetByIdAsync(guid));

        /// <summary>
        /// Retrieves a paginated list of all modules.
        /// </summary>
        /// <param name="page">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
        /// <returns>A paginated list of modules.</returns>
        [HttpGet]
        [Authorize(Roles = "Teacher,Student")]
        [SwaggerOperation(
            Summary = "Get all modules",
            Description = "Retrieves a list of all modules in the system."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResultDto<ModuleDto>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PaginatedResultDto<ModuleDto>>> GetModules([FromQuery] int page = 1, [FromQuery] int pageSize = 10) =>
            Ok(await _serviceManager.ModuleService.GetAllAsync(page, pageSize));

        /// <summary>
        /// Creates a new module.
        /// </summary>
        /// <param name="module">The details of the module to create.</param>
        /// <returns>The created module.</returns>
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
        [HttpPut("{guid}")]
        [Authorize(Roles = "Teacher")]
        [SwaggerOperation(
            Summary = "Update an existing module",
            Description = "Updates the details of an existing module identified by its GUID."
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateModule(Guid guid, [FromBody] UpdateModuleDto module)
        {
            await _serviceManager.ModuleService.UpdateAsync(guid, module);
            return NoContent();
        }

        /// <summary>
        /// Deletes a module by its unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier of the module to delete.</param>
        [HttpDelete("{guid}")]
        [Authorize(Roles = "Teacher")]
        [SwaggerOperation(
            Summary = "Delete a module",
            Description = "Deletes the module identified by its GUID."
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteModule(Guid guid)
        {
            await _serviceManager.ModuleService.DeleteAsync(guid);
            return NoContent();
        }
    }
}
