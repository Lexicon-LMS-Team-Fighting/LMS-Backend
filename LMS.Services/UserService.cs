using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Exceptions;
using Domain.Models.Entities;
using LMS.Shared.DTOs.AuthDtos;
using LMS.Shared.DTOs.UserDtos;
using Microsoft.AspNetCore.Identity;
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
	UserManager<ApplicationUser> _userManager;
	RoleManager<IdentityRole> _roleManager;

	/// <summary>
	/// Initializes a new instance of the <see cref="UserService"/> class.
	/// </summary>
	/// <param name="unitOfWork">The unit of work for repository access.</param>
	/// <param name="mapper">The AutoMapper instance for mapping domain entities to DTOs.</param>
	public UserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_userManager = userManager;
		_roleManager = roleManager;
    }

	/// <inheritdoc/>
	public async Task<UserExtendedDto> GetUserAsync(string userId)
	{
		if (!Guid.TryParse(userId, out Guid guid))
			throw new BadRequestException($"Provided id: {userId} is not a valid Guid");

		var user = await _unitOfWork.User.GetUserAsync(userId);

		if (user is null) throw new UserNotFoundException(guid); 
		
		return _mapper.Map<UserExtendedDto>(user);
	}

	/// <inheritdoc/>
	public async Task<IEnumerable<UserPreviewDto>> GetUsersAsync()
	{
		var user = await _unitOfWork.User.GetUsersAsync();

		return _mapper.Map<IEnumerable<UserPreviewDto>>(user);
	}
}
