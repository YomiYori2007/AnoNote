using System.ComponentModel.DataAnnotations;

namespace PetProject.Application.DTOs.Auth;

public class LoginDto
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }
}