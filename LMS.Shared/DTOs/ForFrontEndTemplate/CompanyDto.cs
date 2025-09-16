namespace LMS.Shared.DTOs.ForFrontEndTemplate;

//Just for demo in frontend Template
public class CompanyDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public IEnumerable<EmployeeDto>? Employees { get; set; }
}

public class EmployeeDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required int Age { get; set; }
    public required string Position { get; set; }
}
