using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Domain.Models.Exceptions.Authorization;
using Domain.Models.Exceptions.BadRequest;
using Domain.Models.Exceptions.Conflict;
using Domain.Models.Exceptions.NotFound;
using LMS.Shared.DTOs.LMSActivityDtos;
using LMS.Shared.DTOs.LMSActivityFeedbackDtos;
using Service.Contracts;
using System.Diagnostics;

namespace LMS.Services
{
    /// <summary>
    /// Service implementation for managing <see cref="LMSActivityFeedbackService"/> entities.
    /// </summary>
    public class LMSActivityFeedbackService : ILMSActivityFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LMSActivityFeedbackService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work for accessing repositories.</param>
        /// <param name="mapper">The AutoMapper instance for mapping entities and DTOs.</param>
        /// <param name="currentUserService">The service for accessing current user information.</param>
        public LMSActivityFeedbackService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        /// <inheritdoc />
        public async Task<LMSActivityFeedbackExtendedDto> GetByIdAsync(Guid id)
        {
            LMSActivity? activity = null;

            if (_currentUserService.IsTeacher)
                activity = await _unitOfWork.LMSActivityFeedback.GetByIdAsync(id);
            else if (_currentUserService.IsStudent)
                activity = await _unitOfWork.LMSActivityFeedback.GetByIdAsync(id, _currentUserService.Id);
            else
                throw new UserRoleNotSupportedException();

            if (activity is null)
                throw new LMSActivityNotFoundException(id);

            return _mapper.Map<LMSActivityExtendedDto>(activity);
        }

        /// <inheritdoc />
        /// <exception cref="LMSActivityFeedbackAlreadyExistsException">Thrown when feedback already exists for the given activity and user.</exception>
        /// <exception cref="LMSActivityNotFoundException">Thrown when the specified activity does not exist.</exception>
        /// <exception cref="UserRoleNotSupportedException">Thrown when the user role is not supported for this operation.</exception>
        public async Task<LMSActivityFeedbackExtendedDto> CreateAsync(CreateLMSActivityFeedbackDto createDto)
        {
            if (await _unitOfWork.LMSActivityFeedback.ExistsAsync(createDto.ActivityId, createDto.UserId))
                throw new LMSActivityFeedbackAlreadyExistsException(createDto.ActivityId, createDto.UserId);

            var activity = await _unitOfWork.LMSActivity.GetByIdAsync(createDto.ActivityId, null);
            if (activity is null)
                throw new LMSActivityNotFoundException(createDto.ActivityId);

            if (!await _unitOfWork.User.IsUserStudentAsync(createDto.UserId))
                throw new UserRoleNotSupportedException("Provided user Id does not belong to a student.");

            if (!await _unitOfWork.LMSActivity.IsUserEnrolledInActivityAsync(createDto.ActivityId, createDto.UserId))
                throw new UserNotEnrolledInActivityException(createDto.UserId, createDto.ActivityId);

            var feedbackEntity = _mapper.Map<LMSActivityFeedback>(createDto);
            _unitOfWork.LMSActivityFeedback.Create(feedbackEntity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<LMSActivityFeedbackExtendedDto>(feedbackEntity);
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }


        public async Task UpdateAsync(Guid activityId, string userId, UpdateLMSActivityFeedbackDto updateDto)
        {
            if (await _unitOfWork.LMSActivityFeedback.ExistsAsync(createDto.ActivityId, createDto.UserId))
                throw new LMSActivityFeedbackAlreadyExistsException(createDto.ActivityId, createDto.UserId);

            var activity = await _unitOfWork.LMSActivity.GetByIdAsync(createDto.ActivityId, null);
            if (activity is null)
                throw new LMSActivityNotFoundException(createDto.ActivityId);

            if (!await _unitOfWork.User.IsUserStudentAsync(createDto.UserId))
                throw new UserRoleNotSupportedException("Provided user Id does not belong to a student.");

            if (!await _unitOfWork.LMSActivity.IsUserEnrolledInActivityAsync(createDto.ActivityId, createDto.UserId))
                throw new UserNotEnrolledInActivityException(createDto.UserId, createDto.ActivityId);

            var feedbackEntity = _mapper.Map<LMSActivityFeedback>(createDto);
            _unitOfWork.LMSActivityFeedback.Create(feedbackEntity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<LMSActivityFeedbackExtendedDto>(feedbackEntity);
        }
    }
}
