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

        /// <summary>
        /// Retrieves an activity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the activity.</param>
        /// <returns>A <see cref="LMSActivityDetailedDto"/> representing the activity.</returns>
        /// <exception cref="LMSActivityNotFoundException">Thrown if the activity is not found.</exception>
        /// <exception cref="UserRoleNotSupportedException">Thrown if the current user's role is not supported.</exception>
        public async Task<LMSActivityDetailedDto> GetByIdAsync(Guid id)
        {
            LMSActivity? activity = null;

            if (_currentUserService.IsTeacher)
                activity = await _unitOfWork.LMSActivity.GetByIdAsync(id, true);
            else if (_currentUserService.IsStudent)
                activity = await _unitOfWork.LMSActivity.GetByIdAsync(id, _currentUserService.Id, true);
            else
                throw new UserRoleNotSupportedException();

            if (activity is null)
                throw new LMSActivityNotFoundException(id);

            return _mapper.Map<LMSActivityDetailedDto>(activity);
        }

        /// <summary>
        /// Retrieves a paginated list of all activities.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="PaginatedResultDto{LMSActivityDto}"/> containing the paginated list of activities.</returns>
        public async Task<PaginatedResultDto<LMSActivityDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var activities = await _unitOfWork.LMSActivity.GetAllAsync();


            var paginatedActivities = activities.ToPaginatedResult(new PagingParameters
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            return _mapper.Map<PaginatedResultDto<LMSActivityDto>>(paginatedActivities);
        }

        /// <summary>
        /// Retrieves a paginated list of activities associated with a specific module.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the parent module.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="PaginatedResultDto{LMSActivityDto}"/> containing the paginated list of activities for the specified module.</returns>
        /// <exception cref="ModuleNotFoundException">Thrown if the module is not found.</exception>
        public async Task<PaginatedResultDto<LMSActivityDto>> GetAllByModuleIdAsync(Guid moduleId, int pageNumber, int pageSize)
        {
            if (await _unitOfWork.Module.GetByIdAsync(moduleId) is null)
                throw new ModuleNotFoundException(moduleId);

            var activities = await _unitOfWork.LMSActivity.GetByModuleIdAsync(moduleId);

            var paginatedActivities = activities.ToPaginatedResult(new PagingParameters
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            return _mapper.Map<PaginatedResultDto<LMSActivityDto>>(paginatedActivities);
        }

        /// <summary>
        /// Creates a new activity.
        /// </summary>
        /// <param name="activity">The data for the activity to create.</param>
        /// <returns>A <see cref="LMSActivityDto"/> representing the created activity.</returns>
        /// <exception cref="ModuleNotFoundException">Thrown if the associated module is not found.</exception>
        /// <exception cref="LMSActivityNameAlreadyExistsException">Thrown if the activity name is not unique within the module.</exception>
        /// <exception cref="InvalidDateRangeException">Thrown if the start date is greater than or equal to the end date.</exception>
        /// <exception cref="InvalidLMSActivityDateRangeException">Thrown if the activity dates are outside the module's date range.</exception>
        public async Task<LMSActivityDto> CreateAsync(CreateLMSActivityDto activity)
        {
            var activityEntity = _mapper.Map<LMSActivity>(activity);

            var module = await _unitOfWork.Module.GetByIdAsync(activity.ModuleId);

            if (module is null)
                throw new ModuleNotFoundException(activity.ModuleId);

            var activityType = await _unitOfWork.ActivityType.GetByIdAsync(activity.ActivityTypeId);

            if (activityType is null)
                throw new ActivityTypeNotFoundException(activity.ActivityTypeId);

            activityEntity.ActivityTypeId = activityType.Id;

            if (!await IsUniqueNameAsync(activity.Name, activity.ModuleId))
                throw new LMSActivityNameAlreadyExistsException(activity.Name, activity.ModuleId);

            if (activity.StartDate > activity.EndDate)
                throw new InvalidDateRangeException(activity.StartDate, activity.EndDate);

            if (activity.StartDate < module.StartDate || activity.EndDate > module.EndDate)
                throw new InvalidLMSActivityDateRangeException();

            _unitOfWork.LMSActivity.Create(activityEntity);
            await _unitOfWork.CompleteAsync();

            // To load Activity type entity
            activityEntity = await _unitOfWork.LMSActivity.GetByIdAsync(activityEntity.Id);

            return _mapper.Map<LMSActivityDto>(activityEntity);
        }

        /// <summary>
        /// Deletes an activity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the activity to delete.</param>
        /// <exception cref="LMSActivityNotFoundException">Thrown if the activity is not found.</exception>
        public async Task DeleteAsync(Guid id)
        {
            var activity = await _unitOfWork.LMSActivity.GetByIdAsync(id, true);

            if (activity is null)
                throw new LMSActivityNotFoundException(id);

            _unitOfWork.LMSActivity.Delete(activity);
            await _unitOfWork.CompleteAsync();
        }

        /// <summary>
        /// Updates an existing activity.
        /// </summary>
        /// <param name="id">The unique identifier of the activity to update.</param>
        /// <param name="updateDto">The updated data for the activity.</param>
        /// <exception cref="LMSActivityNotFoundException">Thrown if the activity is not found.</exception>
        /// <exception cref="ModuleNotFoundException">Thrown if the module is not found.</exception>
        /// <exception cref="LMSActivityNameAlreadyExistsException">Thrown if the updated activity name is not unique within the module.</exception>
        /// <exception cref="InvalidDateRangeException">Thrown if the updated start date is greater than or equal to the end date.</exception>
        /// <exception cref="InvalidLMSActivityDateRangeException">Thrown if the activity dates are outside the module's date range.</exception>
        public async Task UpdateAsync(Guid id, UpdateLMSActivityDto updateDto)
        {
            var activity = await _unitOfWork.LMSActivity.GetByIdAsync(id, true);

            if (activity is null)
                throw new LMSActivityNotFoundException(id);

            var moduleId = updateDto.ModuleId ?? activity.ModuleId;
            var module = await _unitOfWork.Module.GetByIdAsync(moduleId);

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
                if (!await IsUniqueNameAsync(updateDto.Name, activity.ModuleId, activity.Id))
                    throw new LMSActivityNameAlreadyExistsException(updateDto.Name, activity.ModuleId);

                activity.Name = updateDto.Name;
            }

            if (updateDto.Description is not null)
                activity.Description = updateDto.Description;

            if (updateDto.StartDate.HasValue)
                activity.StartDate = updateDto.StartDate.Value;

            if (updateDto.EndDate.HasValue)
                activity.EndDate = updateDto.EndDate.Value;

            if (activity.StartDate > activity.EndDate)
                throw new InvalidDateRangeException(activity.StartDate, activity.EndDate);

            if (activity.StartDate < module.StartDate || activity.EndDate > module.EndDate)
                throw new InvalidLMSActivityDateRangeException();

            _unitOfWork.LMSActivity.Update(activity);

            await _unitOfWork.CompleteAsync();
        }

        /// <summary>
        /// Checks if an activity name is unique within a specific module, excluding a specific activity if provided.
        /// </summary>
        /// <param name="name">The name of the activity to check.</param>
        /// <param name="moduleId">The unique identifier of the parent module.</param>
        /// <param name="excludedActivityId">
        /// The unique identifier of an activity to exclude from the uniqueness check (optional).
        /// Use this parameter when updating an activity to avoid conflicts with its current name.
        /// </param>
        /// <returns>
        /// <c>true</c> if the activity name is unique within the module; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> IsUniqueNameAsync(string name, Guid moduleId, Guid excludedActivityId = default)
        {
            var activities = await _unitOfWork.LMSActivity.GetByModuleIdAsync(moduleId);

            return !activities.Any(activity =>
                activity.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                activity.Id != excludedActivityId);
        }
    }
}
