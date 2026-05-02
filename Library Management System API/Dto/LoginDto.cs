using System.ComponentModel.DataAnnotations;

namespace Library_Management_System_API.Dto;

public class LoginDto
{

    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}