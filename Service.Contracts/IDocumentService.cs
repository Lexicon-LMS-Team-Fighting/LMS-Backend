using LMS.Shared.DTOs.DocumentDtos;
using LMS.Shared.DTOs.PaginationDtos;

namespace Service.Contracts
{
    /// <summary>
    /// Service contract for managing documents.
    /// Provides methods for creating, retrieving, updating, and deleting documents,
    /// as well as retrieving paginated lists of documents.
    /// </summary>
    public interface IDocumentService
    {
        /// <summary>
        /// Retrieves a paginated list of document previews.
        /// </summary>
        /// <param name="page">The current page number (starting from 1).</param>
        /// <param name="pageSize">The number of documents per page.</param>
        /// <returns>
        /// A <see cref="PaginatedResultDto{T}"/> containing a collection of <see cref="DocumentPreviewDto"/>
        /// and pagination metadata.
        /// </returns>
        Task<PaginatedResultDto<DocumentPreviewDto>> GetAllAsync(int page, int pageSize);

        /// <summary>
        /// Retrieves a paginated list of document previews associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose documents are to be retrieved.</param>
        /// <param name="page">The page number to retrieve. Must be greater than or equal to 1.</param>
        /// <param name="pageSize">The number of items per page. Must be greater than or equal to 1.</param>
        /// <returns>A <see cref="PaginatedResultDto{T}"/> containing a collection of <see cref="DocumentPreviewDto"/>  objects
        /// for the specified page and page size.</returns>
        Task<PaginatedResultDto<DocumentPreviewDto>> GetAllByUserIdAsync(string userId, int page, int pageSize);

        /// <summary>
        /// Retrieves detailed information about a specific document by its unique identifier.
        /// </summary>
        /// <param name="documentId">The unique identifier of the document.</param>
        /// <returns>
        /// A <see cref="DocumentExtendedDto"/> containing the document's metadata, relations,
        /// and any extended details.
        /// </returns>
        Task<DocumentExtendedDto> GetByIdAsync(Guid documentId);

        /// <summary>
        /// Creates a new document based on the provided data transfer object.
        /// </summary>
        /// <param name="createDto">The data used to create the document.</param>
        /// <returns>
        /// A <see cref="DocumentExtendedDto"/> representing the newly created document.
        /// </returns>
        Task<DocumentExtendedDto> CreateAsync(CreateDocumentDto createDto);

        /// <summary>
        /// Updates metadata of an existing document.
        /// </summary>
        /// <param name="documentId">The unique identifier of the document to update.</param>
        /// <param name="updateDto">The data used to update the document's metadata (e.g. title, description).</param>
        /// <returns>A task representing the asynchronous update operation.</returns>
        Task UpdateAsync(Guid documentId, UpdateDocumentDto updateDto);

        /// <summary>
        /// Deletes a document by its unique identifier.
        /// </summary>
        /// <param name="documentId">The unique identifier of the document to delete.</param>
        /// <returns>A task representing the asynchronous delete operation.</returns>
        Task DeleteAsync(Guid documentId);

        /// <summary>
        /// Attaches an existing document to a specific course.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="documentId">The unique identifier of the document to attach.</param>
        /// <returns>A task representing the asynchronous attach operation.</returns>
        Task AttachToCourseAsync(Guid courseId, Guid documentId);

        /// <summary>
        /// Removes a document from a specific course.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="documentId">The unique identifier of the document to remove.</param>
        /// <returns>A task representing the asynchronous detach operation.</returns>
        Task DetachFromCourseAsync(Guid courseId, Guid documentId);

        /// <summary>
        /// Attaches an existing document to a specific module.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="documentId">The unique identifier of the document to attach.</param>
        /// <returns>A task representing the asynchronous attach operation.</returns>
        Task AttachToModuleAsync(Guid moduleId, Guid documentId);

        /// <summary>
        /// Removes a document from a specific module.
        /// </summary>
        /// /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="documentId">The unique identifier of the document to remove.</param>
        /// <returns>A task representing the asynchronous detach operation.</returns>
        Task DetachFromModuleAsync(Guid moduleId, Guid documentId);

        /// <summary>
        /// Attaches an existing document to a specific activity.
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity.</param>
        /// <param name="documentId">The unique identifier of the document to attach.</param>
        /// <returns>A task representing the asynchronous attach operation.</returns>
        Task AttachToActivityAsync(Guid activityId, Guid documentId);

        /// <summary>
        /// Removes a document from a specific activity.
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity.</param>
        /// <param name="documentId">The unique identifier of the document to remove.</param>
        /// <returns>A task representing the asynchronous detach operation.</returns>
        Task DetachFromActivityAsync(Guid activityId, Guid documentId);

        /// <summary>
        /// Retrieves a paginated list of document previews associated with the specified activity.
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity for which to retrieve document previews.</param>
        /// <param name="page">The page number to retrieve. Must be greater than or equal to 1.</param>
        /// <param name="pageSize">The number of items to include per page. Must be greater than or equal to 1.</param>
        /// <returns>A <see cref="PaginatedResultDto{T}"/> containing a collection of <see cref="DocumentPreviewDto"/> objects 
        /// for the specified page and page size. If no documents are found, the collection will be empty.</returns>
        Task<PaginatedResultDto<DocumentPreviewDto>> GetAllByActivityIdAsync(Guid activityId, int page, int pageSize);

        /// <summary>
        /// Retrieves a paginated list of document previews associated with the specified module.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module whose documents are to be retrieved.</param>
        /// <param name="page">The page number to retrieve. Must be greater than or equal to 1.</param>
        /// <param name="pageSize">The number of items to include per page. Must be greater than 0.</param>
        /// <returns>A <see cref="PaginatedResultDto{T}"/> containing a collection of <see cref="DocumentPreviewDto"/> objects 
        /// for the specified page and page size. If no documents are found, the collection will be empty.</returns>
        Task<PaginatedResultDto<DocumentPreviewDto>> GetAllByModuleIdAsync(Guid moduleId, int page, int pageSize);

        /// <summary>
        /// Retrieves a paginated list of document previews associated with a specific course.
        /// </summary>
        /// <remarks>This method is asynchronous and returns a task that represents the operation. The
        /// caller must await the task  to retrieve the result. If no documents are found for the specified course, the
        /// returned collection will be empty.</remarks>
        /// <param name="courseId">The unique identifier of the course for which to retrieve documents.</param>
        /// <param name="page">The page number to retrieve. Must be greater than or equal to 1.</param>
        /// <param name="pageSize">The number of items to include per page. Must be greater than or equal to 1.</param>
        /// <returns>A <see cref="PaginatedResultDto{T}"/> containing a collection of <see cref="DocumentPreviewDto"/> objects 
        /// for the specified course, along with pagination metadata.</returns>
        Task<PaginatedResultDto<DocumentPreviewDto>> GetAllByCourseIdAsync(Guid courseId, int page, int pageSize);

        /// <summary>
        /// Retrieves the file path of a document based on its unique identifier.
        /// </summary>
        /// <remarks>The returned file path is relative to the application's root directory. Ensure the
        /// document exists  before calling this method to avoid unexpected results.</remarks>
        /// <param name="documentId">The unique identifier of the document whose path is to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the file path of the document 
        /// as a string.</returns>
        Task<string> GetDocumentFilePathAsync(Guid documentId);
    }

}
