namespace Domain.Models.Entities;


/// <summary>
/// Represents a document within the learning management domain.
/// A document is uploaded or associated with a user, and may belong
/// to a course, module, or activity.
/// 
/// Has the following relations: 
/// Optional M:1 with <see cref="User"/>
/// Optional M:1 with <see cref="Course"/>
/// Optional M:1 with <see cref="Module"/>
/// Optional M:1 with <see cref="Activity"/>
/// </summary>
public class Document
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public Guid CourseId { get; set; }
	public Guid ModuleId { get; set; }
	public Guid ActivityId { get; set; }
	public string Path { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public DateTime TimeStamp { get; set; }

	// Todo: un-comment relations when relations are added
	//public User? User { get; set; }
	//public Course? Course { get; set; }
	//public Module? Module { get; set; }
	//public Activity? Activity { get; set; }
}
