namespace Domain.Contracts.Repositories;

public interface IUnitOfWork
{
	/// <summary>
	/// Gets the repository for handling <see cref="Models.Entities.ApplicationUser"/> entities.
	/// </summary>
	IUserRepository User { get; }
	
	/// <summary>
	/// Gets the repository for handling <see cref="Models.Entities.Course"/> entities.
	/// </summary>
	ICourseRepository Course { get; }

    /// <summary>
	///	Gets the repository for handling <see cref="Models.Entities.Module"/> entities.
	/// </summary>
    IModuleRepository Module { get; }

    /// <summary>
    /// Gets the repository for handling <see cref="Models.Entities.Document"/> entities.
    /// </summary>
    IDocumentRepository Document { get; }

    /// <summary>
    /// Persists all changes made through the repositories in a single transaction.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation.</returns>
    Task CompleteAsync();
}