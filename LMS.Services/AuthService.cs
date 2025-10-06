using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Exceptions;
using Domain.Models.Configurations;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using LMS.Shared.DTOs.AuthDtos;
using LMS.Shared.DTOs.UserDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LMS.Services;
public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JwtSettings _jwtSettings;
    private ApplicationUser? _user;

    public AuthService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<JwtSettings> jwtSettings
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<TokenDto> CreateTokenAsync(bool addTime)
    {
        SigningCredentials signing = GetSigningCredentials();
        IEnumerable<Claim> claims = await GetClaimsAsync();
        JwtSecurityToken token = GenerateToken(signing, claims);

        ArgumentNullException.ThrowIfNull(_user);
        _user.RefreshToken = GenerateRefreshToken();

        if (addTime)
            _user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(3);

        var res = await _userManager.UpdateAsync(_user);
        if (!res.Succeeded) throw new Exception(string.Join("/n", res.Errors));

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return new TokenDto(jwt, _user.RefreshToken!);
    }

    private string? GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private JwtSecurityToken GenerateToken(SigningCredentials signing, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
                                    issuer: _jwtSettings.Issuer,
                                    audience: _jwtSettings.Audience,
                                    claims: claims,
                                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.Expires)),
                                    signingCredentials: signing);

        return token;
    }

    private async Task<IEnumerable<Claim>> GetClaimsAsync()
    {
        ArgumentNullException.ThrowIfNull(_user);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, _user.UserName!),
            new Claim(ClaimTypes.NameIdentifier, _user.Id.ToString())
            //Add more if needed
        };

        var roles = await _userManager.GetRolesAsync(_user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;

    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    public async Task<bool> ValidateUserAsync(UserAuthDto userDto)
    {
        ArgumentNullException.ThrowIfNull(userDto);

        _user = await _userManager.FindByNameAsync(userDto.UserName);

        return _user != null && await _userManager.CheckPasswordAsync(_user, userDto.Password);
    }

    public async Task<TokenDto> RefreshTokenAsync(TokenDto token)
    {
        ClaimsPrincipal principal = GetPrincipalFromExpiredToken(token.AccessToken);
        ApplicationUser? user = await _userManager.FindByNameAsync(principal.Identity?.Name!);

        if (user == null)
            throw new RefreshTokenUserMissingException();

        if (user!.RefreshToken != token.RefreshToken)
            throw new RefreshTokenMismatchException();

        if (user.RefreshTokenExpireTime <= DateTime.Now)
            throw new RefreshTokenExpiredException();

        this._user = user;

        return await CreateTokenAsync(addTime: false);

    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey))
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        ClaimsPrincipal principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }


    /// <inheritdoc />
    public async Task<UserExtendedDto> CreateUserAsync(CreateUserDto createDto)
    {
        if (!await _unitOfWork.User.IsUniqueEmailAsync(createDto.Email))
            throw new UserAlreadyExistsException(createDto.Email, true);

        if (!await _unitOfWork.User.IsUniqueUsernameAsync(createDto.UserName))
            throw new UserAlreadyExistsException(createDto.UserName);

        if (!await _roleManager.RoleExistsAsync("Student"))
            throw new BadRequestException("Default role 'Student' does not exist in the system.");

        var user = _mapper.Map<ApplicationUser>(createDto);
        IdentityResult result = await _userManager.CreateAsync(user, createDto.Password);

        if (!result.Succeeded)
            throw new UserOperationException(result.Errors.Select(e => e.Description));

        result = await _userManager.AddToRoleAsync(user, "Student");

        if (!result.Succeeded)
        {
            await _userManager.DeleteAsync(user); // Rollback user creation if role assignment fails
            throw new UserOperationException(result.Errors.Select(e => e.Description));
        }

        return _mapper.Map<UserExtendedDto>(user);
    }

    /// <inheritdoc />
    public async Task UpdateUserAsync(string userId, UpdateUserDto updateDto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new UserNotFoundException(userId);

        if (updateDto.Email is not null)
        {
            if (!await _unitOfWork.User.IsUniqueEmailAsync(updateDto.Email, userId))
                throw new UserAlreadyExistsException(updateDto.Email, true);

            user.Email = updateDto.Email;
        }

        if (updateDto.UserName is not null)
        {
            if (!await _unitOfWork.User.IsUniqueUsernameAsync(updateDto.UserName, userId))
                throw new UserAlreadyExistsException(updateDto.UserName);

            user.UserName = updateDto.UserName;
        }

        if (updateDto.FirstName is not null)
            user.FirstName = updateDto.FirstName;

        if (updateDto.LastName is not null)
            user.LastName = updateDto.LastName;

        if (updateDto.Password is not null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResult = await _userManager.ResetPasswordAsync(user, token, updateDto.Password);
            if (!passwordResult.Succeeded)
                throw new UserOperationException(passwordResult.Errors.Select(e => e.Description));
        }

        IdentityResult result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new UserOperationException(result.Errors.Select(e => e.Description));
    }
}
