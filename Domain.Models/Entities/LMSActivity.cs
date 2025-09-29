namespace Domain.Models.Entities;

/// <summary>
/// Represents a learning management system (LMS) activity. <br />
/// An activity belongs to a module, is categorized by an <see cref="ActivityType"/>, <br />
/// and may contain related documents. <br />
/// 
/// The entity has: <br />
/// M:1 relationship with <see cref="Module"/>. <br />
/// M:1 relationship with <see cref="ActivityType"/> <br />
/// 1:M relationship with <see cref="Document"/> <br />
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

    // Foreign Keys / Navigation Properties
    public ActivityType ActivityType { get; set; } = null!;
	public Module Module { get; set; } = null!;
	public ICollection<Document> Documents { get; set; } = new List<Document>();
	public ICollection<LMSActivityFeedback> LMSActivityFeedbacks { get; set; } = new List<LMSActivityFeedback>();
}
