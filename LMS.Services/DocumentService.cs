using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Domain.Models.Exceptions.Authorization;
using Domain.Models.Exceptions.BadRequest;
using Domain.Models.Exceptions.Conflict;
using Domain.Models.Exceptions.NotFound;
using LMS.Shared.DTOs.DocumentDtos;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.Pagination;
using Service.Contracts;
using Document = Domain.Models.Entities.Document;
using Module = Domain.Models.Entities.Module;

namespace LMS.Services
{
    /// <summary>
    /// Service implementation for managing documents.
    /// Provides methods for creating, retrieving, updating, and deleting documents
    /// </summary>
    public class DocumentService : IDocumentService
    {
        private static readonly string _storageDirectory = Path.Combine(AppContext.BaseDirectory, "UploadedFiles");
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work for accessing repositories.</param>
        /// <param name="mapper">The AutoMapper instance for mapping entities and DTOs.</param>
        public DocumentService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;

            if (!Directory.Exists(_storageDirectory))
                Directory.CreateDirectory(_storageDirectory);
        }

        /// <inheritdoc />
        public async Task<DocumentExtendedDto> GetByIdAsync(Guid documentId)
        {
            var document = await GetDocumentByIdAsync(documentId, false);
            return _mapper.Map<DocumentExtendedDto>(document);
        }

        /// <inheritdoc />
        /// <exception cref="UserRoleNotSupportedException">Thrown if the current user's role is not supported for this operation.</exception>
        public async Task<PaginatedResultDto<DocumentPreviewDto>> GetAllAsync(int page, int pageSize)
        {
            var documents = _currentUserService.IsTeacher
                ? await _unitOfWork.Document.GetAllAsync()
                : _currentUserService.IsStudent
                    ? await _unitOfWork.Document.GetAllAsync(_currentUserService.Id)
                    : throw new UserRoleNotSupportedException();

            return ToPaginatedResultDto(documents, page, pageSize);
        }

        /// <inheritdoc />
        public async Task<PaginatedResultDto<DocumentPreviewDto>> GetAllByUserIdAsync(string userId, int page, int pageSize)
        {
            var documents = await _unitOfWork.Document.GetAllAsync(userId);
            return ToPaginatedResultDto(documents, page, pageSize);
        }

        /// <inheritdoc />
        public async Task<PaginatedResultDto<DocumentPreviewDto>> GetAllByActivityIdAsync(Guid activityId, int page, int pageSize)
        {
            if (_currentUserService.IsStudent && !await _unitOfWork.LMSActivity.IsUserEnrolledInActivityAsync(activityId, _currentUserService.Id))
                throw new RoleMismatchException("Only participants of the activity can view its documents.");

            var documents = await _unitOfWork.Document.GetAllByActivityIdAsync(activityId);
            return ToPaginatedResultDto(documents, page, pageSize);
        }

        /// <inheritdoc />
        public async Task<PaginatedResultDto<DocumentPreviewDto>> GetAllByModuleIdAsync(Guid moduleId, int page, int pageSize)
        {
            if (_currentUserService.IsStudent && !await _unitOfWork.Module.IsUserEnrolledInModuleAsync(moduleId, _currentUserService.Id))
                throw new RoleMismatchException("Only participants of the module can view its documents.");

            var documents = await _unitOfWork.Document.GetAllByModuleIdAsync(moduleId);
            return ToPaginatedResultDto(documents, page, pageSize);
        }

        /// <inheritdoc />
        public async Task<PaginatedResultDto<DocumentPreviewDto>> GetAllByCourseIdAsync(Guid courseId, int page, int pageSize)
        {
            if (_currentUserService.IsStudent && !await _unitOfWork.Course.IsUserEnrolledInCourseAsync(courseId, _currentUserService.Id))
                throw new RoleMismatchException("Only participants of the course can view its documents.");

            var documents = await _unitOfWork.Document.GetAllByCourseIdAsync(courseId);
            return ToPaginatedResultDto(documents, page, pageSize);
        }

        /// <inheritdoc />
        public async Task<DocumentExtendedDto> CreateAsync(CreateDocumentDto createDto)
        {
            var document = _mapper.Map<Document>(createDto);

            var fileName = $"{document.Id}_{createDto.File.FileName}";
            var filePath = Path.Combine(_storageDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await createDto.File.CopyToAsync(stream);
            }

            document.UserId = _currentUserService.Id;
            document.Path = filePath;

            _unitOfWork.Document.Create(document);
            await _unitOfWork.CompleteAsync();

            document = await GetDocumentByIdAsync(document.Id, false);

            return _mapper.Map<DocumentExtendedDto>(document);
        }

        public async Task<string> GetDocumentFilePathAsync(Guid documentId)
        {
            var document = await GetDocumentByIdAsync(documentId, false);

            if (!File.Exists(document.Path))
                throw new DocumentFileNotFoundException(documentId);

            return document.Path;
        }

        /// <inheritdoc />
        public async Task UpdateAsync(Guid documentId, UpdateDocumentDto updateDto)
        {
            var document = await GetDocumentByIdAsync(documentId, true);

            if (updateDto.Name is not null)
                document.Name = updateDto.Name;

            if (updateDto.Description is not null)
                document.Description = updateDto.Description;

            _unitOfWork.Document.Update(document);
            await _unitOfWork.CompleteAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Guid documentId)
        {
            var document = await GetDocumentByIdAsync(documentId, true);
            _unitOfWork.Document.Delete(document);
            await _unitOfWork.CompleteAsync();
        }

        /// <inheritdoc />
        /// <exception cref="DocumentAlreadyAttachedException">Thrown if the document is already attached to a course.</exception>
        public async Task AttachToCourseAsync(Guid courseId, Guid documentId)
        {
            var document = await GetDocumentByIdAsync(documentId, true);

            await EnsureCourseExistsAsync(courseId);

            if (!await _unitOfWork.Course.IsUserEnrolledInCourseAsync(courseId, _currentUserService.Id))
                throw new RoleMismatchException("Only participants of the course can attach documents to it.");

            if (document.CourseId is not null)
                throw new DocumentAlreadyAttachedException(documentId, document.CourseId.Value, nameof(Course));

            if (document.CourseId == courseId)
                return;

            document.CourseId = courseId;

            _unitOfWork.Document.Update(document);
            await _unitOfWork.CompleteAsync();
        }

        /// <inheritdoc />
        /// <exception cref="DocumentNotAttachedException">Thrown if the document is not attached to any course.</exception>
        public async Task DetachFromCourseAsync(Guid courseId, Guid documentId)
        {
            var document = await GetDocumentByIdAsync(documentId, true);

            if (document.CourseId is null)
                throw new DocumentNotAttachedException(documentId, nameof(Course));

            if (document.CourseId != courseId)
                throw new DocumentNotAttachedException(documentId, nameof(Course), courseId);

            document.CourseId = null;

            _unitOfWork.Document.Update(document);
            await _unitOfWork.CompleteAsync();
        }

        /// <inheritdoc />
        /// <exception cref="DocumentAlreadyAttachedException">Thrown if the document is already attached to a module.</exception>
        public async Task AttachToModuleAsync(Guid moduleId, Guid documentId)
        {
            var document = await GetDocumentByIdAsync(documentId, true);

            await EnsureModuleExistsAsync(moduleId);

            if (!await _unitOfWork.Module.IsUserEnrolledInModuleAsync(moduleId, _currentUserService.Id))
                throw new RoleMismatchException("Only participants of the module can attach documents to it.");

            if (document.ModuleId is not null)
                throw new DocumentAlreadyAttachedException(documentId, document.ModuleId.Value, nameof(Module));

            if (document.ModuleId == moduleId)
                return;

            document.ModuleId = moduleId;

            _unitOfWork.Document.Update(document);
            await _unitOfWork.CompleteAsync();
        }

        /// <inheritdoc />
        /// <exception cref="DocumentNotAttachedException">Thrown if the document is not attached to any module.</exception>
        public async Task DetachFromModuleAsync(Guid moduleId, Guid documentId)
        {
            var document = await GetDocumentByIdAsync(documentId, true);

            if (document.ModuleId is null)
                throw new DocumentNotAttachedException(documentId, nameof(Module));

            if (document.ModuleId != moduleId)
                throw new DocumentNotAttachedException(documentId, nameof(Module), moduleId);

            document.ModuleId = null;

            _unitOfWork.Document.Update(document);
            await _unitOfWork.CompleteAsync();
        }

        /// <inheritdoc />
        /// <exception cref="DocumentAlreadyAttachedException">Thrown if the document is already attached to an activity.</exception>
        public async Task AttachToActivityAsync(Guid activityId, Guid documentId)
        {
            var document = await GetDocumentByIdAsync(documentId, true);

            await EnsureActivityExistsAsync(activityId);

            if (!await _unitOfWork.LMSActivity.IsUserEnrolledInActivityAsync(activityId, _currentUserService.Id))
                throw new RoleMismatchException("Only participants of the activity can attach documents to it.");

            if (document.ActivityId is not null)
                throw new DocumentAlreadyAttachedException(documentId, document.ActivityId.Value, nameof(LMSActivity));

            if (document.ActivityId == activityId)
                return;

            document.ActivityId = activityId;

            _unitOfWork.Document.Update(document);
            await _unitOfWork.CompleteAsync();
        }

        /// <inheritdoc />
        /// <exception cref="DocumentNotAttachedException">Thrown if the document is not attached to any activity.</exception>
        public async Task DetachFromActivityAsync(Guid activityId, Guid documentId)
        {
            var document = await GetDocumentByIdAsync(documentId, true);

            if (document.ActivityId is null)
                throw new DocumentNotAttachedException(documentId, nameof(LMSActivity));

            if (document.ActivityId != activityId)
                throw new DocumentNotAttachedException(documentId, nameof(LMSActivity), activityId);

            document.ActivityId = null;

            _unitOfWork.Document.Update(document);
            await _unitOfWork.CompleteAsync();
        }

        /// <summary>
        /// Retrieves a document by its unique identifier, with access restrictions based on the current user's role.
        /// </summary>
        /// <remarks>The method enforces role-based access control: <list type="bullet"> <item>
        /// <description>If the current user is a teacher, the document is retrieved without additional
        /// restrictions.</description> </item> <item> <description>If the current user is a student, the document is
        /// retrieved only if it is associated with the student's ID.</description> </item> <item> <description>If the
        /// current user's role is unsupported, an exception is thrown.</description> </item> </list></remarks>
        /// <param name="documentId">The unique identifier of the document to retrieve.</param>
        /// <param name="requireOwnershipForStudents">If <c>true</c>, restricts access to documents owned by the current user when the user is a student.</param>
        /// <returns>The <see cref="Document"/> object corresponding to the specified <paramref name="documentId"/>.</returns>
        /// <exception cref="UserRoleNotSupportedException">Thrown if the current user's role is not supported for this operation.</exception>
        /// <exception cref="DocumentNotFoundException">Thrown if no document with the specified <paramref name="documentId"/> is found.</exception>
        private async Task<Document> GetDocumentByIdAsync(Guid documentId, bool requireOwnershipForStudents)
        {
            Document? document;

            if (_currentUserService.IsTeacher)
            {
                document = await _unitOfWork.Document.GetByIdAsync(documentId);
            }
            else if (_currentUserService.IsStudent)
            {
                if (!requireOwnershipForStudents)
                    throw new RoleMismatchException("Students can only access their own documents.");

                document = await _unitOfWork.Document.GetByIdAsync(documentId, _currentUserService.Id);
            }
            else
            {
                throw new UserRoleNotSupportedException();
            }

            if (document is null)
                throw new DocumentNotFoundException(documentId);

            return document;
        }

        /// <summary>
        /// Ensures that a course with the specified identifier exists and is accessible to the current user.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course to verify.</param>
        /// <returns></returns>
        /// <exception cref="UserRoleNotSupportedException">Thrown if the current user's role is not supported for this operation.</exception>
        /// <exception cref="CourseNotFoundException">Thrown if the course with the specified <paramref name="courseId"/> does not exist or is not accessible to
        /// the current user.</exception>
        private async Task EnsureCourseExistsAsync(Guid courseId)
        {
            var document = _currentUserService.IsTeacher
                ? await _unitOfWork.Course.GetCourseAsync(courseId, null)
                : _currentUserService.IsStudent
                    ? await _unitOfWork.Course.GetCourseAsync(courseId, _currentUserService.Id, null)
                    : throw new UserRoleNotSupportedException();

            if (document is null)
                throw new CourseNotFoundException(courseId);
        }

        /// <summary>
        /// Ensures that a module with the specified ID exists and is accessible to the current user.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module to check.</param>
        /// <returns></returns>
        /// <exception cref="UserRoleNotSupportedException">Thrown if the current user's role is not supported.</exception>
        /// <exception cref="ModuleNotFoundException">Thrown if no module is found with the specified ID.</exception>
        private async Task EnsureModuleExistsAsync(Guid moduleId)
        {
            var document = _currentUserService.IsTeacher
                ? await _unitOfWork.Module.GetByIdAsync(moduleId, null)
                : _currentUserService.IsStudent
                    ? await _unitOfWork.Module.GetByIdAsync(moduleId, _currentUserService.Id, null)
                    : throw new UserRoleNotSupportedException();

            if (document is null)
                throw new ModuleNotFoundException(moduleId);
        }

        /// <summary>
        /// Ensures that an activity with the specified identifier exists and is accessible to the current user.
        /// </summary>
        /// <remarks>This method checks whether the activity exists and is accessible based on the current
        /// user's role. If the user is a teacher, the activity is checked without restrictions. If the user is a
        /// student,  the activity is checked for accessibility specific to the student's context.</remarks>
        /// <param name="activityId">The unique identifier of the activity to verify.</param>
        /// <returns></returns>
        /// <exception cref="UserRoleNotSupportedException">Thrown if the current user's role is not supported for this operation.</exception>
        /// <exception cref="LMSActivityNotFoundException">Thrown if the activity with the specified <paramref name="activityId"/> does not exist or is not accessible
        /// to the current user.</exception>
        private async Task EnsureActivityExistsAsync(Guid activityId)
        {
            var document = _currentUserService.IsTeacher
                ? await _unitOfWork.LMSActivity.GetByIdAsync(activityId, null)
                : _currentUserService.IsStudent
                    ? await _unitOfWork.LMSActivity.GetByIdAsync(activityId, _currentUserService.Id, null)
                    : throw new UserRoleNotSupportedException();

            if (document is null)
                throw new LMSActivityNotFoundException(activityId);
        }

        /// <summary>
        /// Converts a collection of documents into a paginated result DTO.
        /// </summary>
        /// <remarks>This method uses the provided collection of documents and applies pagination based on
        /// the specified page number and page size. The resulting paginated data is then mapped to a DTO
        /// representation.</remarks>
        /// <param name="documents">The collection of documents to paginate and convert.</param>
        /// <param name="page">The page number to retrieve. Must be greater than or equal to 1.</param>
        /// <param name="pageSize">The number of items per page. Must be greater than or equal to 1.</param>
        /// <returns>A <see cref="PaginatedResultDto{T}"/> containing the paginated and mapped results.</returns>
        private PaginatedResultDto<DocumentPreviewDto> ToPaginatedResultDto(IEnumerable<Document> documents, int page, int pageSize)
        {
            var paginatedList = documents.ToPaginatedResult(new PagingParameters
            {
                PageNumber = page,
                PageSize = pageSize
            });
            return _mapper.Map<PaginatedResultDto<DocumentPreviewDto>>(paginatedList);
        }
    }
}