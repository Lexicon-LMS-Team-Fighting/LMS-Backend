using AutoMapper;
using Domain.Contracts.Repositories;
using LMS.Shared.DTOs.UserDtos;
using Service.Contracts;

namespace LMS.Services;

public class UserService : IUserService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public UserService(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public Task<UserDto> GetUserAsync(string userId)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<UserDto>> GetUsersAsync()
	{
		throw new NotImplementedException();
	}
}
