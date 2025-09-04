using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetProject.Application.DTOs.Auth;
using PetProject.Application.Models;
using PetProject.Application.Services.Impl;
using PetProject.Application.Services.Interfaces;
using PetProject.Domain.Entities;

namespace PetProject.Web.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly RabbitMqService _rabbitMqService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;

    public AuthController(UserManager<ApplicationUser> userManager, IJwtService jwtService, 
        RabbitMqService rabbitMqService)
    {
        _rabbitMqService = rabbitMqService;
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
        var user = new ApplicationUser { UserName = dto.Username, Email = dto.Email };
        var result = await _userManager.CreateAsync(user, dto.Password);
        
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        try
        {
            var emailMessage = new EmailMessage
            {
                ToEmail = dto.Email,
                Subject = "Добро пожаловать на форум AnoNote!",
                Body = $@"Мы рады вас приветствовать, {dto.Username}!
        
Спасибо, что присоединились к нашему проекту. Вот несколько советов для начала:
- Можете создать публичную заметку
- Или пообсуждать существующие заметки
- Уважайте других пользователей и наслаждайтесь!

Если у вас есть вопросы, просто ответьте на это письмо.

Команда AnoNote"
            };
            
            _rabbitMqService.SendMessage(emailMessage);

            return Ok(new { message = "Регистрация успешна. Приветственное письмо отправлено." });
        }
        catch 
        {
            return Ok(new { message = "Регистрация успешна, но не удалось отправить приветственное письмо." });
        }
    }
}