using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions.Authorization;
using Domain.Models.Exceptions.Conflict;
using Domain.Models.Exceptions.NotFound;
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
        /// <exception cref="LMSActivityFeedbackNotFoundException">Thrown when no feedback is found for the given activity and user.</exception>
        /// <exception cref="UserRoleNotSupportedException">Thrown when the user role is not supported for this operation.</exception>
        public async Task<LMSActivityFeedbackExtendedDto> GetByActivityAndUserIdAsync(Guid activityId, string userId)
        {
            LMSActivityFeedback? feedback = null;

            if (_currentUserService.IsTeacher)
            {
                feedback = await _unitOfWork.LMSActivityFeedback.GetByActivityAndUserIdAsync(activityId, userId);
            }
            else if (_currentUserService.IsStudent)
            {
                if (userId != _currentUserService.Id)
                    throw new UserRoleNotSupportedException("Students can only access their own feedback.");

                feedback = await _unitOfWork.LMSActivityFeedback.GetByActivityAndUserIdAsync(activityId, userId);
            }
            else
                throw new UserRoleNotSupportedException();

            if (feedback is null)
                throw new LMSActivityFeedbackNotFoundException(activityId, userId);

            return _mapper.Map<LMSActivityFeedbackExtendedDto>(feedback);
        }

        /// <inheritdoc />
        /// <exception cref="LMSActivityFeedbackAlreadyExistsException">Thrown when feedback already exists for the given activity and user.</exception>
        /// <exception cref="LMSActivityNotFoundException">Thrown when the specified activity does not exist.</exception>
        /// <exception cref="UserRoleNotSupportedException">Thrown when the user role is not supported for this operation.</exception>
        public async Task<LMSActivityFeedbackExtendedDto> CreateAsync(Guid activityId, string userId, CreateLMSActivityFeedbackDto createDto)
        {
            if (await _unitOfWork.LMSActivityFeedback.ExistsAsync(activityId, userId))
                throw new LMSActivityFeedbackAlreadyExistsException(activityId, userId);

            var activity = await _unitOfWork.LMSActivity.GetByIdAsync(activityId, null);
            if (activity is null)
                throw new LMSActivityNotFoundException(activityId);

            if (!await _unitOfWork.User.IsUserStudentAsync(userId))
                throw new UserRoleNotSupportedException("Provided user Id does not belong to a student.");

            if (!await _unitOfWork.LMSActivity.IsUserEnrolledInActivityAsync(activityId, userId))
                throw new UserNotEnrolledInActivityException(userId, activityId);

            var feedbackEntity = _mapper.Map<LMSActivityFeedback>(createDto);
            feedbackEntity.LMSActivityId = activityId;
            feedbackEntity.UserId = userId;

            _unitOfWork.LMSActivityFeedback.Create(feedbackEntity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<LMSActivityFeedbackExtendedDto>(feedbackEntity);
        }

        /// <inheritdoc />
        /// <exception cref="LMSActivityFeedbackNotFoundException">Thrown when no feedback is found for the given activity and user.</exception>
        public async Task DeleteAsync(Guid activityId, string userId)
        {
            var feedback = await _unitOfWork.LMSActivityFeedback.GetByActivityAndUserIdAsync(activityId, userId);

            if (feedback is null)
                throw new LMSActivityFeedbackNotFoundException(activityId, userId);

            _unitOfWork.LMSActivityFeedback.Delete(feedback);
            await _unitOfWork.CompleteAsync();
        }

        /// <inheritdoc />
        /// <exception cref="LMSActivityFeedbackNotFoundException">Thrown when no feedback is found for the given activity and user.</exception>
        public async Task UpdateAsync(Guid activityId, string userId, UpdateLMSActivityFeedbackDto updateDto)
        {
            var feedback = await _unitOfWork.LMSActivityFeedback.GetByActivityAndUserIdAsync(activityId, userId);

            if (feedback is null)
                throw new LMSActivityFeedbackNotFoundException(activityId, userId);

            if (updateDto.Feedback is not null)
                feedback.Feedback = updateDto.Feedback;

            if (updateDto.Status is not null)
                feedback.Status = updateDto.Status;

            _unitOfWork.LMSActivityFeedback.Update(feedback);
            await _unitOfWork.CompleteAsync();
        }
    }
}
