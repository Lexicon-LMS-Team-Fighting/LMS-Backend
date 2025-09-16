using Bogus;
using LMS.Shared.DTOs.ForFrontEndTemplate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.Presentation.DemoController;

//Just for demo
[Route("api/companies")]
[ApiController]
public class CompanyDemoAuthController : ControllerBase
{
    [HttpGet]
    [Authorize]
    [SwaggerOperation(
        Summary =     "Get demo companaies for authenticated users",
        Description = "Returns a list of companies. Requires a valid JWT token.")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of demo companies", typeof(IEnumerable<CompanyDto>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - JWT token missing or invalid")]
    public IActionResult GetDemoAuth()
    {
        return Ok(GenerateCompanies(10));
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Get one demo companay for authenticated users",
        Description = "Returns a demo company. Requires a valid JWT token and a guid Id")]
    [SwaggerResponse(StatusCodes.Status200OK, "A demo Company", typeof(IEnumerable<CompanyDto>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - JWT token missing or invalid")]
    public IActionResult GetCompany(Guid id)
    {
        var dto = new CompanyDto { Id = id, Name = "DemoCompany", Address = "Demovägen 123, Stockholm, Sweden", Employees = GenerateEmployees(2) };
        return Ok(dto);
    }
               
    private static List<CompanyDto> GenerateCompanies(int numberOfCompanies)
    {
        var faker = new Faker<CompanyDto>("sv").Rules((f, c) =>
        {
            c.Id = f.Random.Guid();
            c.Name = f.Company.CompanyName();
            c.Address = $"{f.Address.StreetAddress()}, {f.Address.City()}, {f.Address.Country}";
            c.Employees = GenerateEmployees(f.Random.Int(1, 5));
        });

        return faker.Generate(numberOfCompanies);
    }

    private static List<EmployeeDto> GenerateEmployees(int numberOfEmplyees)
    {
        string[] positions = ["Developer", "Tester", "Manager"];

        var faker = new Faker<EmployeeDto>("sv").Rules((f, e) =>
        {
            e.Id = f.Random.Guid();
            e.Name = f.Person.FullName;
            e.Age = f.Random.Int(min: 18, max: 70);
            e.Position = positions[f.Random.Int(0, positions.Length - 1)];
        });

        return faker.Generate(numberOfEmplyees);
    }
}
