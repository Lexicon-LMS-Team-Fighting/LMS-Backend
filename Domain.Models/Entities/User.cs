

namespace Domain.Models.Entities;
/// <summary>
/// Relations: many-to-many with Course via UserCourse, one-to-many with Document,
/// optional 1:1 link to ApplicationUser.
/// </summary>
public class User
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;

    
    //public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
    //public ICollection<Document> Documents { get; set; } = new List<Document>();

    /* Is this needed?
    public string? ApplicationUserId { get; set; }           
    public ApplicationUser? ApplicationUser { get; set; }    
    */
}