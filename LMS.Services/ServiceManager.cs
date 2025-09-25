using Service.Contracts;

namespace LMS.Services;

public class ServiceManager : IServiceManager
{

    // ToDo: TestService related is to be removed when no longer needed.
    private readonly Lazy<ITestService> _testService;
	private Lazy<IAuthService> _authService;
	private readonly Lazy<IUserService> _userService;
	private readonly Lazy<ICourseService> _courseService;
    private readonly Lazy<IModuleService> _moduleService;
    private readonly Lazy<ILMSActivityService> _lmsActivityService;
    private readonly Lazy<IActivityTypeService> _activityTypeService;

    public ITestService TestService => _testService.Value;
    public IAuthService AuthService => _authService.Value;
    public IUserService UserService => _userService.Value;
    public ICourseService CourseService => _courseService.Value;
    public IModuleService ModuleService => _moduleService.Value;
    public ILMSActivityService LMSActivityService => _lmsActivityService.Value;
    public IActivityTypeService ActivityTypeService => _activityTypeService.Value;

    public ServiceManager(
        Lazy<ITestService> testService,
        Lazy<IAuthService> authService,
        Lazy<IUserService> userService,
        Lazy<ICourseService> courseService,
        Lazy<IModuleService> moduleService,
        Lazy<ILMSActivityService> lmsActivityService,
        Lazy<IActivityTypeService> activityTypeService
        )
    {
        _testService = testService;
        _authService = authService;
        _userService = userService;
		_courseService = courseService;
        _moduleService = moduleService;
        _lmsActivityService = lmsActivityService;
        _activityTypeService = activityTypeService;
    }
}
