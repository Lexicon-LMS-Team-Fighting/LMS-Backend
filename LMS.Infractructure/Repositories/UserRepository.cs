using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
	public UserRepository(ApplicationDbContext context): base(context)
	{}

	// Not used yet. Parameters set to change.
	void IRepositoryBase<User>.Create(User entity)
	{
		throw new NotImplementedException();
	}

	// Not used yet. Parameters set to change.
	void IRepositoryBase<User>.Delete(User entity)
	{
		throw new NotImplementedException();
	}

	async Task<User?> IUserRepository.GetUserAsync(Guid userId, bool changeTracking) => 
		await FindByCondition(u => u.Id.Equals(userId), changeTracking)
				.FirstOrDefaultAsync();


	async Task<List<User>> IUserRepository.GetUsersAsync(bool changeTracking) => 
		await FindAll(changeTracking).ToListAsync();
	

	// Not used yet. Parameters set to change.
	void IRepositoryBase<User>.Update(User entity)
	{
		throw new NotImplementedException();
	}
}
