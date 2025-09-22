namespace Domain.Models.Entities;

/// <summary>
/// Represents a module for a course.
/// <para>
/// This entity has the following relations: <br />
/// 1:M with <see cref="LMSActivity"/>. <br />
/// Optional 1:M with <see cref="Document"/>. <br />
/// M:1 with <see cref="Course"/>. <br />
/// </para>
/// </summary>
public class Module
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Foreign Keys / Navigation Properties
    public Course Course { get; set; } = null!;
    public ICollection<LMSActivity> LMSActivities { get; set; } = new List<LMSActivity>();
    public ICollection<Document> Documents { get; set; } = new List<Document>();
}