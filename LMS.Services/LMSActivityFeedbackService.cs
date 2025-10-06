using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions.Authorization;
using Domain.Models.Exceptions.NotFound;
using LMS.Shared.DTOs.LMSActivityDtos;
using LMS.Shared.DTOs.LMSActivityFeedbackDtos;
using Service.Contracts;

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

        public Task<LMSActivityFeedbackExtendedDto> CreateAsync(CreateLMSActivityFeedbackDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }


        public Task UpdateAsync(Guid id, UpdateLMSActivityFeedbackDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
