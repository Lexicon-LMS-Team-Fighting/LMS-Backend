using Service.Contracts;

namespace LMS.Services;

public class ServiceManager : IServiceManager
{

    // ToDo: TestService related is to be removed when no longer needed.
    private readonly Lazy<ITestService> _testService;
	private Lazy<IAuthService> _authService;
	private readonly Lazy<IUserService> _userService;
	private readonly Lazy<ICourseService> _courseService;

	public ITestService TestService => _testService.Value;
    public IAuthService AuthService => _authService.Value;
    public IUserService UserService => _userService.Value;
    public ICourseService CourseService => _courseService.Value;

	public ServiceManager(
        Lazy<ITestService> testService,
        Lazy<IAuthService> authService,
        Lazy<IUserService> userService,
        Lazy<ICourseService> courseService)
    {
        _testService = testService;
        _authService = authService;
        _userService = userService;
		_courseService = courseService;
	}
}
