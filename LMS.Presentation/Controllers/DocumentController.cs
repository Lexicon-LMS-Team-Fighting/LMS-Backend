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
    using LMS.Shared.DTOs.DocumentDtos;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    namespace YourNamespace.Controllers
    {
        /// <summary>
        /// Controller for managing documents.
        /// Provides endpoints for creating, updating, deleting, and retrieving documents.
        /// </summary>
        [Route("api/documents")]
        [ApiController]
        [Authorize]
        public class DocumentController : ControllerBase
        {
            private readonly IServiceManager _serviceManager;

            /// <summary>
            /// Initializes a new instance of the <see cref="DocumentController"/> class.
            /// </summary>
            /// <param name="serviceManager">The service manager for accessing document-related services.</param>
            public DocumentController(IServiceManager serviceManager)
            {
                _serviceManager = serviceManager;
            }

            /// <summary>
            /// Retrieves all documents (paginated).
            /// </summary>
            /// <param name="page">The page number to retrieve (default is 1).</param>
            /// <param name="pageSize">The number of items per page (default is 10).</param>
            /// <returns>A paginated list of documents.</returns>
            /// <response code="200">Returns a list of documents.</response>
            /// <response code="401">Unauthorized.</response>
            /// <response code="403">Forbidden.</response>
            [HttpGet]
            [Authorize(Roles = "Teacher,Student")]
            [SwaggerOperation(
                Summary = "Get all documents",
                Description = "Retrieves a paginated list of all documents available to the user."
            )]
            [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResultDto<DocumentPreviewDto>))]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status403Forbidden)]
            public async Task<ActionResult<PaginatedResultDto<DocumentPreviewDto>>> GetDocuments(
                [FromQuery] int page = 1,
                [FromQuery] int pageSize = 10)
            {
                var documents = await _serviceManager.DocumentService.GetAllAsync(page, pageSize);
                return Ok(documents);
            }

            /// <summary>
            /// Retrieves a specific document by its unique identifier.
            /// </summary>
            /// <param name="documentId">The unique identifier of the document.</param>
            /// <returns>The requested document.</returns>
            /// <response code="200">Returns the document.</response>
            /// <response code="404">If no document is found with the specified ID.</response>
            /// <response code="401">Unauthorized.</response>
            /// <response code="403">Forbidden.</response>
            [HttpGet("{documentId}")]
            [Authorize(Roles = "Teacher,Student")]
            [SwaggerOperation(
                Summary = "Get document by ID",
                Description = "Retrieves a document and its metadata by its unique ID."
            )]
            [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentExtendedDto))]
            [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status403Forbidden)]
            public async Task<ActionResult<DocumentExtendedDto>> GetDocumentById(Guid documentId)
            {
                var document = await _serviceManager.DocumentService.GetByIdAsync(documentId);
                return Ok(document);
            }

            /// <summary>
            /// Downloads a specific document by its unique identifier.
            /// </summary>
            /// <param name="documentId">The unique identifier of the document to download.</param>
            /// <response code="200">Returns the requested document as a file.</response>
            /// <response code="404">If the document is not found.</response>
            /// <response code="401">Unauthorized.</response>
            /// <response code="403">Forbidden.</response>
            [HttpGet("{documentId}/download")]
            [Authorize(Roles = "Teacher,Student")]
            [SwaggerOperation(
                Summary = "Download a document",
                Description = "Downloads the specified document as a file."
            )]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status403Forbidden)]
            public async Task<IActionResult> DownloadDocument(Guid documentId)
            {
                var path = await _serviceManager.DocumentService.GetDocumentFilePathAsync(documentId);

                var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                var contentType = "application/octet-stream";
                var fileName = Path.GetFileName(path);

                return File(fileStream, contentType, fileName);
            }

            /// <summary>
            /// Creates a new document.
            /// </summary>
            /// <param name="createDto">The document data to create.</param>
            /// <returns>The created document.</returns>
            /// <response code="201">Returns the created document.</response>
            /// <response code="400">If the provided document data is invalid.</response>
            /// <response code="401">Unauthorized.</response>
            /// <response code="403">Forbidden.</response>
            [HttpPost]
            [Authorize(Roles = "Teacher,Student")]
            [SwaggerOperation(
                Summary = "Create a new document",
                Description = "Creates a new document and stores its metadata (title, description, file URL, etc.)."
            )]
            [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(DocumentExtendedDto))]
            [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status403Forbidden)]
            [Consumes("multipart/form-data")]
            public async Task<ActionResult<DocumentExtendedDto>> CreateDocument([FromForm] CreateDocumentDto createDto)
            {
                var created = await _serviceManager.DocumentService.CreateAsync(createDto);
                return CreatedAtAction(nameof(GetDocumentById), new { documentId = created.Id }, created);
            }

            /// <summary>
            /// Updates metadata of an existing document.
            /// </summary>
            /// <param name="documentId">The unique identifier of the document to update.</param>
            /// <param name="updateDto">The updated document data.</param>
            /// <response code="204">Document was successfully updated.</response>
            /// <response code="400">If the provided data is invalid.</response>
            /// <response code="404">If no document is found with the specified ID.</response>
            /// <response code="401">Unauthorized.</response>
            /// <response code="403">Forbidden.</response>
            [HttpPut("{documentId}")]
            [Authorize(Roles = "Teacher,Student")]
            [SwaggerOperation(
                Summary = "Update document metadata",
                Description = "Updates the title, description, or other metadata of an existing document."
            )]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
            [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status403Forbidden)]
            public async Task<IActionResult> UpdateDocument(Guid documentId, [FromBody] UpdateDocumentDto updateDto)
            {
                await _serviceManager.DocumentService.UpdateAsync(documentId, updateDto);
                return NoContent();
            }

            /// <summary>
            /// Deletes a specific document by its unique identifier.
            /// </summary>
            /// <param name="documentId">The unique identifier of the document to delete.</param>
            /// <response code="204">Document was successfully deleted.</response>
            /// <response code="404">If no document is found with the specified ID.</response>
            /// <response code="401">Unauthorized.</response>
            /// <response code="403">Forbidden.</response>
            [HttpDelete("{documentId}")]
            [Authorize(Roles = "Teacher")]
            [SwaggerOperation(
                Summary = "Delete a document",
                Description = "Deletes an existing document and removes all associations with courses, modules, or activities."
            )]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status403Forbidden)]
            public async Task<IActionResult> DeleteDocument(Guid documentId)
            {
                await _serviceManager.DocumentService.DeleteAsync(documentId);
                return NoContent();
            }
        }
    }

}
