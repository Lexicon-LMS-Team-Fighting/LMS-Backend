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

	/// <inheritdoc/>
	public async Task<UserDto> GetUserAsync(string userId)
	{
		if (!Guid.TryParse(userId, out _))
			throw new BadRequestException($"Provided id: {userId} is not a valid Guid");

		var user = await _unitOfWork.User.GetUserAsync(userId);

		if (user is null) throw new UserNotFoundException(userId); 
		
		return _mapper.Map<UserDto>(user);
	}

	/// <inheritdoc/>
	public async Task<IEnumerable<UserDto>> GetUsersAsync()
	{
		var user = await _unitOfWork.User.GetUsersAsync();

		return _mapper.Map<IEnumerable<UserDto>>(user);
	}
}
