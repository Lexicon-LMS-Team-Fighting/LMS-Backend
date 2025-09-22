namespace Domain.Models.Entities;

/// <summary>
/// Represents the relationship between a user and a course.
/// This entity has:  
/// 1:M relationship between <c>UserCourse</c> and <c>User</c>.
/// M:1 relationship between <c>UserCourse</c> and <c>Course</c> 
/// </summary>
public class UserCourse
{
	public string UserId { get; set; } = null!;
	public Guid CourseId { get; set; }

    // Foreign Keys / Navigation Properties
    public ApplicationUser User { get; set; } = null!;
	public Course Course { get; set; } = null!;
}
