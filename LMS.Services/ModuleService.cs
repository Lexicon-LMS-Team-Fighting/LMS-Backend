using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Domain.Models.Exceptions.Authorization;
using Domain.Models.Exceptions.BadRequest;
using Domain.Models.Exceptions.Conflict;
using LMS.Shared.DTOs.CourseDtos;
using LMS.Shared.DTOs.ModuleDtos;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.Pagination;
using Service.Contracts;
using System.Reflection.Metadata.Ecma335;

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

        /// <inheritdoc />
        /// <exception cref="ModuleNotFoundException">Thrown if the module is not found.</exception>
        /// <exception cref="UserRoleNotSupportedException">Thrown when the current user's role is neither Teacher nor Student.</exception>
        public async Task<ModuleExtendedDto> GetByIdAsync(Guid id, string? include)
        {
            Module? module = null;
            bool includeProgress = !string.IsNullOrEmpty(include) && include.Contains(nameof(ModuleExtendedDto.Progress), StringComparison.OrdinalIgnoreCase);
            decimal progress = 0;

            if (_currentUserService.IsTeacher)
            {
                module = await _unitOfWork.Module.GetByIdAsync(id, include, true);

                if (includeProgress)
                    progress = await _unitOfWork.Module.CalculateProgress(id);
            }
            else if (_currentUserService.IsStudent)
            {
                module = await _unitOfWork.Module.GetByIdAsync(id, _currentUserService.Id, include);

                if (includeProgress)
                    progress = await _unitOfWork.Module.CalculateProgress(id, _currentUserService.Id);
            }
            else throw new UserRoleNotSupportedException();

            if (module is null)
                throw new ModuleNotFoundException(id);

            var moduleDto = _mapper.Map<ModuleExtendedDto>(module);

            if (includeProgress)
                moduleDto.Progress = progress;

            return moduleDto;
        }

        /// <inheritdoc />
        /// <exception cref="UserRoleNotSupportedException">Thrown when the current user's role is neither Teacher nor Student.</exception>
        public async Task<PaginatedResultDto<ModulePreviewDto>> GetAllAsync(int pageNumber, int pageSize, string? include = null)
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

            var modulesDto = _mapper.Map<PaginatedResultDto<ModulePreviewDto>>(paginatedModules);

            if (!string.IsNullOrEmpty(include) && include.Contains(nameof(ModulePreviewDto.Progress), StringComparison.OrdinalIgnoreCase))
            {
                await AddProgress(modulesDto);
            }

            return modulesDto;
        }

        /// <inheritdoc />
        /// <exception cref="UserRoleNotSupportedException">Thrown when the current user's role is neither Teacher nor Student.</exception>
        /// <exception cref="CourseNotFoundException">Thrown if the course is not found or the user does not have access to it.</exception>
        public async Task<PaginatedResultDto<ModulePreviewDto>> GetAllByCourseIdAsync(Guid courseId, int pageNumber, int pageSize, string? include = null)
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

            var modulesDto = _mapper.Map<PaginatedResultDto<ModulePreviewDto>>(paginatedModules);

            if (!string.IsNullOrEmpty(include) && include.Contains(nameof(ModulePreviewDto.Progress), StringComparison.OrdinalIgnoreCase))
            {
                await AddProgress(modulesDto);
            }

            return modulesDto;
        }

        /// <summary>
        /// Calculates and adds progress information to each module in the provided paginated result.
        /// </summary>
        /// <param name="modulesDto">The paginated result of module previews to which progress will be added.</param>
        /// <exception cref="UserRoleNotSupportedException"></exception>
        private async Task AddProgress(PaginatedResultDto<ModulePreviewDto> modulesDto)
        {
            Func<Guid, Task<decimal>> calculateProgressFunc = _currentUserService.IsTeacher
                ? (async (moduleId) => await _unitOfWork.Module.CalculateProgress(moduleId))
                : _currentUserService.IsStudent
                    ? (async (moduleId) => await _unitOfWork.Module.CalculateProgress(moduleId, _currentUserService.Id))
                    : throw new UserRoleNotSupportedException();

            foreach (var module in modulesDto.Items)
            {
                module.Progress = await calculateProgressFunc(module.Id);
            }
        }

        /// <inheritdoc />
        /// <returns>A <see cref="ModuleExtendedDto"/> representing the created module.</returns>
        /// <exception cref="CourseNotFoundException">Thrown if the associated course is not found.</exception>
        /// <exception cref="ModuleNameAlreadyExistsException">Thrown if the module name is not unique within the course.</exception>
        /// <exception cref="InvalidDateRangeException">Thrown if the start date is greater than or equal to the end date.</exception>
        /// <exception cref="InvalidModuleDateRangeException">Thrown if the module dates are outside the associated course date range.</exception>
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

        /// <inheritdoc />
        /// <exception cref="ModuleNotFoundException">Thrown if the module is not found.</exception>
        public async Task DeleteAsync(Guid id)
        {
            var module = await _unitOfWork.Module.GetByIdAsync(id, null);

            if (module is null)
                throw new ModuleNotFoundException(id);

            _unitOfWork.Module.Delete(module);
            await _unitOfWork.CompleteAsync();
        }

        /// <inheritdoc />
        /// <exception cref="ModuleNotFoundException">Thrown if the module is not found.</exception>
        /// <exception cref="ModuleNameAlreadyExistsException">Thrown if the updated module name is not unique within the course.</exception>
        /// <exception cref="InvalidDateRangeException">Thrown if the updated start date is greater than or equal to the end date.</exception>
        /// <exception cref="InvalidModuleDateRangeException">Thrown if the updated module dates are outside the associated course date range.</exception>
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

        /// <inheritdoc />
        public async Task<bool> IsUniqueNameAsync(string name, Guid courseId, Guid excludedModuleId = default)
        {
            var modules = await _unitOfWork.Module.GetByCourseIdAsync(courseId);

            return !modules.Any(module =>
                module.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                module.Id != excludedModuleId);
        }
    }
}
