using Domain.Models.Entities;

namespace Domain.Contracts.Repositories;

public interface IUserRepository: IRepositoryBase<User>
{
	Task<User?> GetUserAsync(Guid userId, bool changeTracking = false);
	Task<List<User>> GetUsersAsync(bool changeTracking = false);
}
