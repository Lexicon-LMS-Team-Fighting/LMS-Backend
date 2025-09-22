using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories;

public class UserRepository : RepositoryBase<ApplicationUser>, IUserRepository
{
	public UserRepository(ApplicationDbContext context): base(context)
	{}

	// Not used yet. Parameters set to change.
	void IRepositoryBase<ApplicationUser>.Create(ApplicationUser entity)
	{
		throw new NotImplementedException();
	}

	// Not used yet. Parameters set to change.
	void IRepositoryBase<ApplicationUser>.Delete(ApplicationUser entity)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc/>
	public async Task<ApplicationUser?> GetUserAsync(string userId, bool changeTracking) => 
		await FindByCondition(u => u.Id.Equals(userId), changeTracking)
				.FirstOrDefaultAsync();

	/// <inheritdoc/>
	public async Task<List<ApplicationUser>> GetUsersAsync(bool changeTracking) => 
		await FindAll(changeTracking).ToListAsync();
	

	// Not used yet. Parameters set to change.
	void IRepositoryBase<ApplicationUser>.Update(ApplicationUser entity)
	{
		throw new NotImplementedException();
	}
}
