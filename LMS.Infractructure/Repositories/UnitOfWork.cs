using Domain.Contracts.Repositories;
using LMS.Infractructure.Data;

namespace LMS.Infractructure.Repositories;

/// <summary>
/// Defines a unit of work that encapsulates access to multiple repositories
/// and coordinates changes across them to ensure transactional consistency.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext context;

    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<ICourseRepository> _courseRepository;
    private readonly Lazy<IModuleRepository> _moduleRepository;
    private readonly Lazy<ILMSActivityRepository> _lmsActivityRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(context));
        _courseRepository = new Lazy<ICourseRepository>(() => new CourseRepository(context));
        _moduleRepository = new Lazy<IModuleRepository>(() => new ModuleRepository(context));
        _lmsActivityRepository = new Lazy<ILMSActivityRepository>(() => new LMSActivityRepository(context));
    }

    /// <inheritdoc/>
	public IUserRepository User => _userRepository.Value;
    
    /// <inheritdoc/>
	public ICourseRepository Course => _courseRepository.Value;
    public IModuleRepository Module => _moduleRepository.Value;
    public ILMSActivityRepository LMSActivity => _lmsActivityRepository.Value;

    public async Task CompleteAsync() => await context.SaveChangesAsync();
}
