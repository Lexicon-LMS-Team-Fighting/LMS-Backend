namespace Service.Contracts;
public interface IServiceManager
{
    IAuthService AuthService { get; }
    IUserService UserService { get; }
    ICourseService CourseService { get; }

    ITestService TestService { get; } // ToDo: TestService is to be removed when no longer needed.
}