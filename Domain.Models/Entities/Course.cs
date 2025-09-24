namespace Domain.Models.Entities;

/// <summary>
/// Represents the relationship between a <see cref="Course"/> and <br />
/// <see cref="Module"/>, <see cref="Document"/>, <see cref="UserCourse"/>. 
/// <para>
/// This entity has: <br />
/// 1:M relationship between <see cref="Course"/> and <see cref="Module"/>. <br />
/// 1:M relationship between <see cref="Course"/> and <see cref="Document"/>. <br />
/// 1:M relationship between <see cref="Course"/> and <see cref="UserCourse"/>. <br />
/// </para>
/// </summary>
public class Course
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Foreign Keys / Navigation Properties
    public ICollection<Module> Modules { get; set; } = new List<Module>();
	public Guid CourseId { get; set; }
	public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
    public ICollection<Document>Documents { get; set; } = new List<Document>();
}