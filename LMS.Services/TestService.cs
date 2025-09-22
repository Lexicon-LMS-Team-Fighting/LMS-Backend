using Domain.Contracts.Repositories;
using Service.Contracts;
using System.Threading.Tasks;

namespace LMS.Services;

// ToDo: this class is here for testing purposes only, should be removed when no longer needed.
public class TestService: ITestService
{
	public IUnitOfWork unit;

	public TestService(IUnitOfWork unitOfWork)
	{
		unit = unitOfWork;
		
	}

	public async void TestMethod()
	{
		var result = await unit.User.GetUsersAsync();
		Console.WriteLine("test");
	}
}
