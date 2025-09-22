using Service.Contracts;

namespace LMS.Services;

public class ServiceManager : IServiceManager
{

    // ToDo: TestService related is to be removed when no longer needed.
    private readonly Lazy<ITestService> _testService;
    public ITestService TestService => _testService.Value;


	private Lazy<IAuthService> authService;
    public IAuthService AuthService => authService.Value;

    public ServiceManager(
        Lazy<ITestService> testService,
        Lazy<IAuthService> authService)
    {
        this.authService = authService;
        _testService = testService;
    }
}
