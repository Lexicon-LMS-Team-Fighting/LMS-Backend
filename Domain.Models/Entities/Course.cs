namespace Domain.Models.Entities

    /// <summary>
    /// Represents the relationship between a course and module, course and document.
    /// 
    /// This entity has the following relations: 1:m with Module, 1:m with Document.
    /// </summary>
{
    public class Course
    {
        public Guid CourseId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        // Relations
        // TODO: Add relations
        //public ICollection<Module> Modules { get; set; } = new List<Module>();
        //public ICollection<Document>? Documents { get; set; } = new List<Document>();
        //public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
    }
}
