using LMS.Shared.DTOs.LMSActivityDtos;
using LMS.Shared.DTOs.PaginationDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Presentation.Controllers
{
    /// <summary>
    /// Controller for managing activity types.
    /// Provides endpoints for retrieving available activity types.
    /// </summary>
    [Route("api/activity-types")]
    [ApiController]
    public class ActivityTypeController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityTypeController"/> class.
        /// </summary>
        /// <param name="serviceManager">The service manager for accessing activity type-related services.</param>
        public ActivityTypeController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Retrieves a list of all activity types.
        /// </summary>
        /// <returns>A list of activity types.</returns>
        /// <response code="200">Returns the list of activity types.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all activity types",
            Description = "Retrieves all activity types available in the system."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<string>))]
        public async Task<ActionResult<IEnumerable<string>>> GetActivityTypes() =>
            Ok(await _serviceManager.ActivityTypeService.GetAllAsync());
    }

}
