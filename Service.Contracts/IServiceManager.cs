namespace Service.Contracts;
public interface IServiceManager
{
    IAuthService AuthService { get; }
    ITestService TestService { get; } // ToDo: TestService is to be removed when no longer needed.
}