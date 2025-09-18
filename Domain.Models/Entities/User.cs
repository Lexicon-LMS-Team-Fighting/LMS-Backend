

namespace Domain.Models.Entities;
/// <summary>
/// Relations: many-to-many with Course via UserCourse, one-to-many with Document
/// </summary>
public class User
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;


    //public UserCourse UserCourse { get; set; } = null!;
    //public ICollection<Document> Documents { get; set; } = new List<Document>();


}