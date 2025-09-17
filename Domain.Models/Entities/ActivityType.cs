using System.Diagnostics;

namespace Domain.Models.Entities;

/// <summary>
/// Represents a type or category of activity within the domain model.
/// This entity can be used to classify different learning or management activities
/// (e.g., e-learning, assignment, exercise, and so on).
/// has a 1:M relationship with <c>LMSActivity</c>.
/// </summary>
public class ActivityType
{
	public Guid id { get; set; }
	public string name { get; set; } = string.Empty;

	//public ICollection<LMSActivity> LMSActivities { get; set; } = new List<LMSActivity>();
}
