namespace Domain.Models.Entities;

/// <summary>
/// Represents a learning management system (LMS) activity.
/// An activity belongs to a module, is categorized by an <see cref="ActivityType"/>,
/// and may contain related documents.
/// 
/// The entity has:
/// M:1 relationship with <see cref="Module"/>.
/// M:1 relationship with <see cref="ActivityType"/>
/// 1:M relationship with <see cref="Document"/>
/// </summary>
public class LMSActivity
{
	public Guid Id { get; set; }
	public Guid ModuleId { get; set; }
	public Guid ActivityTypeId { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }

	// Todo: Un-comment bellow relation variables when implementing relations
	//[ForeignKey(nameof(ActivityTypeId))]
	//public ActivityType ActivityType { get; set; }
	//public ICollection<Document> Documents { get; set; } = new List<Document>();
}
