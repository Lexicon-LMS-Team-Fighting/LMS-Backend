namespace Domain.Models.Entities;

/// <summary>
/// Represents the relationship between a user and a course.
/// This entity has:  
/// 1:M relationship between <c>UserCourse</c> and <c>User</c>.
/// M:1 relationship between <c>UserCourse</c> and <c>Course</c> 
/// </summary>
public class UserCourse
{
	public Guid UserId { get; set; }
	public Guid CourseId { get; set; }
	//public ICollection<User> Users { get; set; } = new List<User>();
	//public Course Course { get; set; }
}									
