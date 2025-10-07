using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Domain.Models.Exceptions.Authorization;
using Domain.Models.Exceptions.BadRequest;
using Domain.Models.Exceptions.Conflict;
using Domain.Models.Exceptions.NotFound;
using LMS.Shared.DTOs.LMSActivityDtos;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.Pagination;
using Service.Contracts;
using System.Reflection;

namespace LMS.Services
{
    /// <summary>
    /// Service implementation for managing <see cref="LMSActivity"/> entities.
    /// Provides methods for creating, retrieving, updating, and deleting activities,
    /// as well as additional functionality such as pagination and name uniqueness checks.
    /// </summary>
    public class LMSActivityService : ILMSActivityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LMSActivityService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work for accessing repositories.</param>
        /// <param name="mapper">The AutoMapper instance for mapping entities and DTOs.</param>
        /// <param name="currentUserService">The service for accessing current user information.</param>
        public LMSActivityService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        /// <inheritdoc />
        /// <exception cref="LMSActivityNotFoundException">Thrown if the activity is not found.</exception>
        /// <exception cref="UserRoleNotSupportedException">Thrown if the current user's role is not supported.</exception>
        public async Task<LMSActivityExtendedDto> GetByIdAsync(Guid id, string? include)
        {
            LMSActivity? activity = null;

            if (_currentUserService.IsTeacher)
                activity = await _unitOfWork.LMSActivity.GetByIdAsync(id, include);
            else if (_currentUserService.IsStudent)
                activity = await _unitOfWork.LMSActivity.GetByIdAsync(id, _currentUserService.Id, include);
            else
                throw new UserRoleNotSupportedException();

            if (activity is null)
                throw new LMSActivityNotFoundException(id);

            return _mapper.Map<LMSActivityExtendedDto>(activity);
        }

        /// <inheritdoc />
        /// <exception cref="UserRoleNotSupportedException">Thrown when the current user's role is neither Teacher nor Student.</exception>
        public async Task<PaginatedResultDto<LMSActivityPreviewDto>> GetAllAsync(PaginatedQueryDto queryDto)
        {
            PaginatedResult<LMSActivity> paginatedActivities;

            if (_currentUserService.IsTeacher)
            {
                paginatedActivities = await _unitOfWork.LMSActivity.GetAllAsync(queryDto);
            }
            else if (_currentUserService.IsStudent)
            {
                paginatedActivities = await _unitOfWork.LMSActivity.GetAllAsync(_currentUserService.Id, queryDto);
            }
            else throw new UserRoleNotSupportedException();

            return _mapper.Map<PaginatedResultDto<LMSActivityPreviewDto>>(paginatedActivities);
        }

        /// <inheritdoc />
        /// <exception cref="UserRoleNotSupportedException">Thrown when the current user's role is neither Teacher nor Student.</exception>
        /// <exception cref="ModuleNotFoundException">Thrown if the module is not found or the user does not have access to it.</exception>
        public async Task<PaginatedResultDto<LMSActivityPreviewDto>> GetAllByModuleIdAsync(Guid moduleId, PaginatedQueryDto queryDto)
        {
            PaginatedResult<LMSActivity> paginatedActivities;

            if (_currentUserService.IsTeacher)
            {
                if (await _unitOfWork.Module.GetByIdAsync(moduleId, null) is null)
                    throw new ModuleNotFoundException(moduleId);

                paginatedActivities = await _unitOfWork.LMSActivity.GetByModuleIdAsync(moduleId, queryDto);
            }
            else if (_currentUserService.IsStudent)
            {
                if (await _unitOfWork.Module.GetByIdAsync(moduleId, _currentUserService.Id) is null)
                    throw new ModuleNotFoundException(moduleId);

                paginatedActivities = await _unitOfWork.LMSActivity.GetByModuleIdAsync(moduleId, _currentUserService.Id, queryDto);
            }
            else
                throw new UserRoleNotSupportedException();

            return _mapper.Map<PaginatedResultDto<LMSActivityPreviewDto>>(paginatedActivities);
        }

        /// <inheritdoc />
        /// <exception cref="ModuleNotFoundException">Thrown if the associated module is not found.</exception>
        /// <exception cref="LMSActivityNameAlreadyExistsException">Thrown if the activity name is not unique within the module.</exception>
        /// <exception cref="InvalidDateRangeException">Thrown if the start date is greater than or equal to the end date.</exception>
        /// <exception cref="InvalidLMSActivityDateRangeException">Thrown if the activity dates are outside the module's date range.</exception>
        public async Task<LMSActivityExtendedDto> CreateAsync(CreateLMSActivityDto activity)
        {
            var activityEntity = _mapper.Map<LMSActivity>(activity);

            var module = await _unitOfWork.Module.GetByIdAsync(activity.ModuleId, null);

            if (module is null)
                throw new ModuleNotFoundException(activity.ModuleId);

            var activityType = await _unitOfWork.ActivityType.GetByIdAsync(activity.ActivityTypeId);

            if (activityType is null)
                throw new ActivityTypeNotFoundException(activity.ActivityTypeId);

            activityEntity.ActivityTypeId = activityType.Id;

            if (!await _unitOfWork.LMSActivity.IsUniqueNameAsync(activity.Name, activity.ModuleId))
                throw new LMSActivityNameAlreadyExistsException(activity.Name, activity.ModuleId);

            if (activity.StartDate >= activity.EndDate)
                throw new InvalidDateRangeException(activity.StartDate, activity.EndDate);

            if (activity.StartDate < module.StartDate || activity.EndDate > module.EndDate)
                throw new InvalidLMSActivityDateRangeException();

            _unitOfWork.LMSActivity.Create(activityEntity);
            await _unitOfWork.CompleteAsync();

            // To load Activity type entity
            activityEntity = await _unitOfWork.LMSActivity.GetByIdAsync(activityEntity.Id, null);

            return _mapper.Map<LMSActivityExtendedDto>(activityEntity);
        }

        /// <inheritdoc />
        /// <exception cref="LMSActivityNotFoundException">Thrown if the activity is not found.</exception>
        /// <exception cref="ModuleNotFoundException">Thrown if the module is not found.</exception>
        /// <exception cref="LMSActivityNameAlreadyExistsException">Thrown if the updated activity name is not unique within the module.</exception>
        /// <exception cref="InvalidDateRangeException">Thrown if the updated start date is greater than or equal to the end date.</exception>
        /// <exception cref="InvalidLMSActivityDateRangeException">Thrown if the activity dates are outside the module's date range.</exception>
        public async Task UpdateAsync(Guid id, UpdateLMSActivityDto updateDto)
        {
            var activity = await _unitOfWork.LMSActivity.GetByIdAsync(id, null, true);

            if (activity is null)
                throw new LMSActivityNotFoundException(id);

            var moduleId = updateDto.ModuleId ?? activity.ModuleId;
            var module = await _unitOfWork.Module.GetByIdAsync(moduleId, null);

            if (module is null)
                throw new ModuleNotFoundException(moduleId);

            if (updateDto.ModuleId.HasValue)
                activity.ModuleId = updateDto.ModuleId.Value;

            if (updateDto.ActivityTypeId.HasValue)
            {
                var activityType = await _unitOfWork.ActivityType.GetByIdAsync(updateDto.ActivityTypeId.Value);

                if (activityType is null)
                    throw new ActivityTypeNotFoundException(updateDto.ActivityTypeId.Value);

                activity.ActivityTypeId = activityType.Id;
            }

            if (updateDto.ActivityTypeId.HasValue)
            {
                var activityType = await _unitOfWork.ActivityType.GetByIdAsync(updateDto.ActivityTypeId.Value);

                if (activityType is null)
                    throw new ActivityTypeNotFoundException(updateDto.ActivityTypeId.Value);

                activity.ActivityTypeId = activityType.Id;
            }

            if (updateDto.Name is not null)
            {
                if (!await _unitOfWork.LMSActivity.IsUniqueNameAsync(updateDto.Name, activity.ModuleId, activity.Id))
                    throw new LMSActivityNameAlreadyExistsException(updateDto.Name, activity.ModuleId);

                activity.Name = updateDto.Name;
            }

            if (updateDto.Description is not null)
                activity.Description = updateDto.Description;

            if (updateDto.StartDate.HasValue)
                activity.StartDate = updateDto.StartDate.Value;

            if (updateDto.EndDate.HasValue)
                activity.EndDate = updateDto.EndDate.Value;

            if (activity.StartDate >= activity.EndDate)
                throw new InvalidDateRangeException(activity.StartDate, activity.EndDate);

            if (activity.StartDate < module.StartDate || activity.EndDate > module.EndDate)
                throw new InvalidLMSActivityDateRangeException();

            _unitOfWork.LMSActivity.Update(activity);

            await _unitOfWork.CompleteAsync();
        }

        /// <inheritdoc />
        /// <exception cref="LMSActivityNotFoundException">Thrown if the activity is not found.</exception>
        public async Task DeleteAsync(Guid activityId)
        {
            var activity = await _unitOfWork.LMSActivity.GetByIdAsync(activityId, null);

            if (activity is null)
                throw new LMSActivityNotFoundException(activityId);

            await _unitOfWork.LMSActivity.ClearDocumentRelationsAsync(activityId);
            await _unitOfWork.CompleteAsync();
            _unitOfWork.DetachAllEntities();

            _unitOfWork.LMSActivity.Delete(activity);
            await _unitOfWork.CompleteAsync();
        }
    }
}
