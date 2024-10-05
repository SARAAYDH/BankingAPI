using Banking.Service.Dtos;
using Banking.Service.Helpers;
using Microsoft.AspNetCore.Identity;
namespace Banking.Service.Services;

public interface IUserService
{
    Task<AuthResult> RegisterUserAsync(RegisterDto registerDto);
    Task<AuthResult> LoginUserAsync(LoginDto loginDto);
    Task<string> GenerateJwtToken(IdentityUser user);
}
