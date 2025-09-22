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
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;

	public ICollection<LMSActivity> LMSActivities { get; set; } = new List<LMSActivity>();
}
