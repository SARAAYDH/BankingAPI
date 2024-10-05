using Banking.Service.Dtos;
using Banking.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Banking.API.Controllers;

[ApiController]
[Route("/v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Register")]
    [SwaggerOperation("RegisterUser")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        try
        {
            var result = await _userService.RegisterUserAsync(registerDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }
        catch
        {
            return StatusCode(500, "Internal server error.");
        }
    }
    [HttpPost("Login")]
    [SwaggerOperation("Login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        try 
        { 
            var result = await _userService.LoginUserAsync(loginDto);
            if (!result.Success)
            {
                return Unauthorized(result.Message);
            }

            return Ok(new { Token = result.Token });
        }
        catch
        {
            return StatusCode(500, "Internal server error.");
        }
    }
}
