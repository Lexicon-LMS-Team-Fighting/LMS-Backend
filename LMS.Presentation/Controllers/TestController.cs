

using LMS.Shared.DTOs.UserDtos;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace LMS.Presentation.Controllers;

// ToDo: this controller class is here for testing purposes only, should be removed when no longer needed.
[Route("api/test")]
[ApiController]
public class TestController: ControllerBase
{
	public IServiceManager _serviceManager;

	public TestController(IServiceManager serviceManager)
	{
		_serviceManager = serviceManager;
	}

	[HttpPost("test")]
	public async Task<bool> RefreshToken(UserDto item)
	{
		_serviceManager.TestService.TestMethod();
		return true;
	}
}
