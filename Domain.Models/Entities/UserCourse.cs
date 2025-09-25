namespace Domain.Models.Entities;

/// <summary>
/// Represents the relationship between a user and a course.<br />
/// This entity has:  <br /><br />
/// 1:M relationship with <see cref="ApplicationUser"/>.<br />
/// M:1 relationship with <see cref="Entities.Course"/>.<br />
/// </summary>
public class UserCourse
{
	public string UserId { get; set; } = null!;
	public Guid CourseId { get; set; }

	// Foreign Keys / Navigation Properties
	public ApplicationUser User { get; set; } = null!;
	public Course Course { get; set; } = null!;
}
