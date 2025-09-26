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
    private readonly Lazy<IDocumentService> _documentService;

    public ITestService TestService => _testService.Value;
    public IAuthService AuthService => _authService.Value;
    public IUserService UserService => _userService.Value;
    public ICourseService CourseService => _courseService.Value;
    public IModuleService ModuleService => _moduleService.Value;
    public IDocumentService DocumentService => _documentService.Value;

    public ServiceManager(
        Lazy<ITestService> testService,
        Lazy<IAuthService> authService,
        Lazy<IUserService> userService,
        Lazy<ICourseService> courseService,
        Lazy<IModuleService> moduleService,
        Lazy<IDocumentService> documentService)
    {
        _testService = testService;
        _authService = authService;
        _userService = userService;
		_courseService = courseService;
        _moduleService = moduleService;
        _documentService = documentService;
    }
}
