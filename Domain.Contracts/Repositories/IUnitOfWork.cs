namespace Domain.Contracts.Repositories;

public interface IUnitOfWork
{
	/// <summary>
	/// Gets the repository for handling <see cref="Models.Entities.User"/> entities.
	/// </summary>
	IUserRepository User { get; }

	/// <summary>
	/// Persists all changes made through the repositories in a single transaction.
	/// </summary>
	/// <returns>A task representing the asynchronous save operation.</returns>
	Task CompleteAsync();
}