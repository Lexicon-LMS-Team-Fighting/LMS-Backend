using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Domain.Models.Exceptions.BadRequest;
using Domain.Models.Exceptions.Conflict;
using Domain.Models.Exceptions.NotFound;
using LMS.Shared.DTOs.ActivityTypeDto;
using LMS.Shared.DTOs.LMSActivityDtos;
using LMS.Shared.DTOs.ModuleDtos;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.Pagination;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services
{
    /// <summary>
    /// Service implementation for managing <see cref="LMSActivity"/> entities.
    /// Provides methods for creating, retrieving, updating, and deleting activities,
    /// as well as additional functionality such as pagination and name uniqueness checks.
    /// </summary>
    public class ActivityTypeService : IActivityTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="LMSActivityService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work for accessing repositories.</param>
        /// <param name="mapper">The AutoMapper instance for mapping entities and DTOs.</param>
        public ActivityTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        /// <summary>
        /// Retrieves a list of all activity types.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{LMSActivityDto}"/> containing the list of all activity types.</returns>
        public async Task<IEnumerable<ActivityTypeDto>> GetAllAsync()
        {
            var activityTypes = await _unitOfWork.ActivityType.GetAllAsync();
            return _mapper.Map<IEnumerable<ActivityTypeDto>>(activityTypes);
        }
    }
}
