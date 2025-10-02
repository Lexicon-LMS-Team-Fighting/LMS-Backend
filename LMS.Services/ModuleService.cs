using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Domain.Models.Exceptions.Authorization;
using Domain.Models.Exceptions.BadRequest;
using Domain.Models.Exceptions.Conflict;
using LMS.Shared.DTOs.ModuleDtos;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.Pagination;
using Service.Contracts;

namespace LMS.Services
{
    /// <summary>
    /// Service implementation for managing modules.
    /// Provides methods for creating, retrieving, updating, and deleting modules,
    /// as well as additional functionality such as pagination and name uniqueness checks.
    /// </summary>
    public class ModuleService : IModuleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work for accessing repositories.</param>
        /// <param name="mapper">The AutoMapper instance for mapping entities and DTOs.</param>
        public ModuleService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Retrieves a module by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the module.</param>
        /// <param name="include">Related entities to include (e.g., "lmsactivities", "participants", "documents").</param>
        /// <returns>A <see cref="ModuleDetailedDto"/> representing the module.</returns>
        /// <exception cref="ModuleNotFoundException">Thrown if the module is not found.</exception>
        public async Task<ModuleExtendedDto> GetByIdAsync(Guid id, string? include)
        {
            Module? module = null;

            if (_currentUserService.IsTeacher)
                module = await _unitOfWork.Module.GetByIdAsync(id, include, true);
            else if (_currentUserService.IsStudent)
                module = await _unitOfWork.Module.GetByIdAsync(id, _currentUserService.Id, include);
            else
                throw new UserRoleNotSupportedException();

            if (module is null)
                throw new ModuleNotFoundException(id);

            return _mapper.Map<ModuleExtendedDto>(module);
        }

        /// <summary>
        /// Retrieves a paginated list of all modules.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="PaginatedResultDto{ModulePreviewDto}"/> containing the paginated list of modules.</returns>
        public async Task<PaginatedResultDto<ModulePreviewDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            IEnumerable<Module>? modules = null;

            if (_currentUserService.IsTeacher)
                modules = await _unitOfWork.Module.GetAllAsync(true);
            else if (_currentUserService.IsStudent)
                modules = await _unitOfWork.Module.GetAllAsync(_currentUserService.Id);
            else
                throw new UserRoleNotSupportedException();

            var paginatedModules = modules.ToPaginatedResult(new PagingParameters
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            return _mapper.Map<PaginatedResultDto<ModulePreviewDto>>(paginatedModules);
        }

        /// <summary>
        /// Retrieves a paginated list of modules associated with a specific course.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="PaginatedResultDto{ModulePreviewDto}"/> containing the paginated list of modules for the specified course.</returns>
        public async Task<PaginatedResultDto<ModulePreviewDto>> GetAllByCourseIdAsync(Guid courseId, int pageNumber, int pageSize)
        {
            IEnumerable<Module>? modules = null;

            if (_currentUserService.IsTeacher)
            {
                if (await _unitOfWork.Course.GetCourseAsync(courseId, null) is null)
                    throw new CourseNotFoundException(courseId);

                modules = await _unitOfWork.Module.GetByCourseIdAsync(courseId, true);
            }
            else if (_currentUserService.IsStudent)
            {
                if (await _unitOfWork.Course.GetCourseAsync(courseId, _currentUserService.Id, null) is null)
                    throw new CourseNotFoundException(courseId);

                modules = await _unitOfWork.Module.GetByCourseIdAsync(courseId, _currentUserService.Id);
            }
            else throw new UserRoleNotSupportedException();

            var paginatedModules = modules.ToPaginatedResult(new PagingParameters
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            return _mapper.Map<PaginatedResultDto<ModulePreviewDto>>(paginatedModules);
        }

        /// <summary>
        /// Creates a new module.
        /// </summary>
        /// <param name="module">The data for the module to create.</param>
        /// <returns>A <see cref="ModuleExtendedDto"/> representing the created module.</returns>
        /// <exception cref="CourseNotFoundException">Thrown if the associated course is not found.</exception>
        /// <exception cref="ModuleNameAlreadyExistsException">Thrown if the module name is not unique within the course.</exception>
        /// <exception cref="InvalidDateRangeException">Thrown if the start date is greater than or equal to the end date.</exception>
        public async Task<ModuleExtendedDto> CreateAsync(CreateModuleDto module)
        {
            var moduleEntity = _mapper.Map<Module>(module);

            var course = await _unitOfWork.Course.GetCourseAsync(module.CourseId, null);

            if (course is null)
                throw new CourseNotFoundException(module.CourseId);

            if (!await IsUniqueNameAsync(module.Name, module.CourseId))
                throw new ModuleNameAlreadyExistsException(module.Name, module.CourseId);

            if (module.StartDate >= module.EndDate)
                throw new InvalidDateRangeException(module.StartDate, module.EndDate);

            if (module.StartDate < course.StartDate || module.EndDate > course.EndDate)
                throw new InvalidModuleDateRangeException();

            _unitOfWork.Module.Create(moduleEntity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ModuleExtendedDto>(moduleEntity);
        }

        /// <summary>
        /// Deletes a module by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the module to delete.</param>
        /// <exception cref="ModuleNotFoundException">Thrown if the module is not found.</exception>
        public async Task DeleteAsync(Guid id)
        {
            var module = await _unitOfWork.Module.GetByIdAsync(id, null);

            if (module is null)
                throw new ModuleNotFoundException(id);

            _unitOfWork.Module.Delete(module);
            await _unitOfWork.CompleteAsync();
        }

        /// <summary>
        /// Updates an existing module.
        /// </summary>
        /// <param name="id">The unique identifier of the module to update.</param>
        /// <param name="updateDto">The updated data for the module.</param>
        /// <exception cref="ModuleNotFoundException">Thrown if the module is not found.</exception>
        /// <exception cref="ModuleNameAlreadyExistsException">Thrown if the updated module name is not unique within the course.</exception>
        /// <exception cref="InvalidDateRangeException">Thrown if the updated start date is greater than or equal to the end date.</exception>
        public async Task UpdateAsync(Guid id, UpdateModuleDto updateDto)
        {
            var module = await _unitOfWork.Module.GetByIdAsync(id, null, true);

            if (module is null)
                throw new ModuleNotFoundException(id);

            var courseId = updateDto.CourseId ?? module.CourseId;
            var course = await _unitOfWork.Course.GetCourseAsync(courseId, null);

            if (course is null)
                throw new CourseNotFoundException(courseId);

            if (updateDto.CourseId is not null)
                module.CourseId = (Guid)updateDto.CourseId;

            if (updateDto.Name is not null)
            {
                if (!await IsUniqueNameAsync(updateDto.Name, module.CourseId, module.Id))
                    throw new ModuleNameAlreadyExistsException(updateDto.Name, module.CourseId);

                module.Name = updateDto.Name;
            }

            if (updateDto.Description is not null)
                module.Description = updateDto.Description;

            if (updateDto.StartDate.HasValue)
                module.StartDate = updateDto.StartDate.Value;

            if (updateDto.EndDate.HasValue)
                module.EndDate = updateDto.EndDate.Value;

            if (module.StartDate >= module.EndDate)
                throw new InvalidDateRangeException(module.StartDate, module.EndDate);

            if (module.StartDate < course.StartDate || module.EndDate > course.EndDate)
                throw new InvalidModuleDateRangeException();

            _unitOfWork.Module.Update(module);
            await _unitOfWork.CompleteAsync();
        }

        /// <summary>
        /// Checks if a module name is unique within a specific course, excluding a specific module if provided.
        /// </summary>
        /// <param name="name">The name of the module to check.</param>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="excludedModuleId">
        /// The unique identifier of a module to exclude from the uniqueness check (optional).
        /// Use this parameter when updating a module to avoid conflicts with its current name.
        /// </param>
        /// <returns>
        /// <c>true</c> if the module name is unique within the course; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> IsUniqueNameAsync(string name, Guid courseId, Guid excludedModuleId = default)
        {
            var modules = await _unitOfWork.Module.GetByCourseIdAsync(courseId);

            return !modules.Any(module =>
                module.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                module.Id != excludedModuleId);
        }
    }
}
