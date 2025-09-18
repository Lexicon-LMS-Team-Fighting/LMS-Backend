using System.Diagnostics;

namespace Domain.Models.Entities;


/// <summary>
/// Represents a document within the learning management domain. <br />
/// A document is uploaded or associated with a user, and may belong <br />
/// to a course, module, or activity. <br />
/// 
/// Has the following relations: <br /> 
/// Optional M:1 with <see cref="User"/> <br />
/// Optional M:1 with <see cref="Course"/> <br />
/// Optional M:1 with <see cref="Module"/> <br />
/// Optional M:1 with <see cref="Activity"/> <br />
/// </summary>
public class Document
{
	public Guid Id { get; set; }
	public string UserId { get; set; } = null!;
	public Guid CourseId { get; set; }
	public Guid ModuleId { get; set; }
	public Guid ActivityId { get; set; }
	public string Path { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public DateTime TimeStamp { get; set; }

	public ApplicationUser User { get; set; } = null!;
	public Course? Course { get; set; }
	public Module? Module { get; set; }
	public LMSActivity? Activity { get; set; }
}
