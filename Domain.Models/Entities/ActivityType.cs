using System.Diagnostics;

namespace Domain.Models.Entities;

/// <summary>
/// Represents a type or category of activity within the domain model. <br />
/// This entity can be used to classify different learning or management activities <br />
/// (e.g., e-learning, assignment, exercise, and so on). <br />
/// has a 1:M relationship with <see cref="LMSActivity"/> <br />
/// </summary>
public class ActivityType
{
	public Guid id { get; set; }
	public string name { get; set; } = string.Empty;

	// ToDo: Un-comment and implement the relationship when LMSActivity is defined.
	//public ICollection<LMSActivity> LMSActivities { get; set; } = new List<LMSActivity>();
}
