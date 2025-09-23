using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories;

/// <summary>
/// Provides data access operations for <see cref="Course"/> entities. <br/>
/// Inherits common CRUD functionality from <see cref="RepositoryBase{T}"/> 
/// and implements course-specific queries defined in <see cref="IUserRepository"/>. <br/>
/// Serves as the concrete repository for managing courses within the system.
/// </summary>
public class CourseRepository : RepositoryBase<Course>, ICourseRepository
{
	public CourseRepository(ApplicationDbContext context) : base(context)
	{}

	/// <inheritdoc/>
	public async Task<Course?> GetCourseAsync(Guid courseId, bool changeTracking = false) => 
		await FindByCondition(c => c.Id.Equals(courseId), changeTracking)
				.FirstOrDefaultAsync();
	
	/// <inheritdoc/>
	public async Task<List<Course>> GetCoursesAsync(bool changeTracking = false) => 
		await FindAll(changeTracking).ToListAsync();
}
