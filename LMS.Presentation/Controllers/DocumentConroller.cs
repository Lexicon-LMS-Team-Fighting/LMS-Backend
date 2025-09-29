using LMS.Shared.DTOs.DocumentDtos;
using LMS.Shared.DTOs.PaginationDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.Presentation.Controllers
{
    /// <summary>
    /// Controller for managing documents.
    /// Provides endpoints for retrieving documents and their details.
    /// </summary>
    [Route("api/documents")]
    [ApiController]
    [Authorize]
    public class DocumentsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentsController"/> class.
        /// </summary>
        /// <param name="serviceManager">The service manager for accessing document-related services.</param>
        public DocumentsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Retrieves a specific document by its unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier of the document.</param>
        /// <returns>A <see cref="DocumentDto"/> representing the document.</returns>
        /// <response code="200">Returns the document details.</response>
        /// <response code="404">If no document is found with the specified GUID.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpGet("{guid}")]
        [Authorize(Roles = "Teacher,Student")]
        [SwaggerOperation(
            Summary = "Get specified document by ID",
            Description = "Retrieves document details by their unique GUID identifier."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<DocumentDto>> GetDocument(Guid guid) =>
            Ok(await _serviceManager.DocumentService.GetByIdAsync(guid));

        /// <summary>
        /// Retrieves a paginated list of all documents.
        /// </summary>
        /// <param name="page">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
        /// <returns>A paginated list of documents.</returns>
        /// <response code="200">Returns a paginated list of documents.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpGet]
        [Authorize(Roles = "Teacher,Student")]
        [SwaggerOperation(
            Summary = "Get all documents",
            Description = "Retrieves a list of all documents in the system."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResultDto<DocumentDto>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PaginatedResultDto<DocumentDto>>> GetDocuments([FromQuery] int page = 1, [FromQuery] int pageSize = 10) =>
            Ok(await _serviceManager.DocumentService.GetAllAsync(page, pageSize));
    }
}
