using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetProject.Application.DTOs.Auth;
using PetProject.Application.Services.Impl;
using PetProject.Domain.Entities;

namespace PetProject.Web.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly JwtService _jwtService;

    public AuthController(UserManager<User> userManager, JwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.Username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return Unauthorized();
        
        var token = _jwtService.GenerateToken(user);
        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = new User { UserName = dto.Username, Email = dto.Email };
        var result = await _userManager.CreateAsync(user, dto.Password);
        
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        
        return Ok();
    }
}