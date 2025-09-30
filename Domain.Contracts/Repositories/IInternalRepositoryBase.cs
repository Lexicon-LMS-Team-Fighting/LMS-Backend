using System.Linq.Expressions;

namespace Domain.Contracts.Repositories;

public interface IInternalRepositoryBase<T>
{
	/// <summary>
	/// Asynchronously determines whether any entities match the given condition.
	/// </summary>
	/// <param name="expression">A LINQ expression used to test against the entities.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains <c>true</c> 
	/// if any entities match the condition; otherwise, <c>false</c>.
	/// </returns>
	Task<bool> FindAnyAsync(Expression<Func<T, bool>> expression);

	IQueryable<T> FindAll(bool trackChanges = false);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);
}
