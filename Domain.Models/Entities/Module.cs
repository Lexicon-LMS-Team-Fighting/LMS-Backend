namespace Domain.Models.Entities

/// <summary>
/// Entity Module
/// Represents the relationship between a Module and Activity, Module and Document, Module and Course.
/// 
/// This entity has the following relations: 1:m with Activity, 1:m with Document.
/// </summary>
{
    public class Module
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Navigation Relations
        // TODO: Add relations 
        //public ICollection<Document> Documents { get; set; } = new List<Document>();
        //public ICollection<Activity> Activities { get; set; } = new List<Activity>();

        // Foreign Key
        // TODO: Add foreign key
        //public Course Course { get; set; }
    }
}