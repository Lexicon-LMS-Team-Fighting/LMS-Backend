namespace Domain.Models.Entities;

/// <summary>
/// Represents the relationship between a user and a course.
/// This entity has a many-to-one relationship between <c>User</c> and <c>Course</c>.
/// </summary>
public class UserCourse
{
	public Guid UserId { get; set; }
	public Guid CourseId { get; set; }
	//public User User { get; set; }
}									
