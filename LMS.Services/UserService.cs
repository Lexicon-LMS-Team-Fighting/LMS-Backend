using AutoMapper;
using Domain.Contracts.Repositories;
using LMS.Shared.DTOs.UserDtos;
using Service.Contracts;

namespace LMS.Services;


/// <summary>
/// Provides operations related to users, including retrieval of single or multiple users.
/// Implements the <see cref="IUserService"/> interface.
/// </summary>
public class UserService : IUserService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	/// <summary>
	/// Initializes a new instance of the <see cref="UserService"/> class.
	/// </summary>
	/// <param name="unitOfWork">The unit of work for repository access.</param>
	/// <param name="mapper">The AutoMapper instance for mapping domain entities to DTOs.</param>
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
