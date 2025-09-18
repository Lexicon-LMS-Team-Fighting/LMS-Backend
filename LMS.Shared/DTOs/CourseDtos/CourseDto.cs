using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.DTOs.CourseDtos;
public class CourseDto
{
    public Guid Id { get; set; }
    [Required]
    [MaxLength(255, ErrorMessage = "Max length is 255.")]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }

    //Relations
    // TODO: Un-comment this one UserDto is implemented.
    //public ICollection<UserDto> Users { get; set; } = new();
    //public ICollection<DocumentDto> Documents { get; set; } = new();
    //public ICollection<ModuleDto> Modules { get; set; } = new();
}
