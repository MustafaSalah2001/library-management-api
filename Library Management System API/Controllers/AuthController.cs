using Library_Management_System_API.Data;
using Library_Management_System_API.Dto;
using Library_Management_System_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Library_Management_System_API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);

        if (emailExists)
        {
            return BadRequest(new
            {
                success = false,
                message = "Email already exists"
            });
        }

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = HashPassword(dto.Password),
            Role = "Member",
            IsApproved = false // 👈 تأكد من وجود هذا السطر بالـ Register
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "User registered successfully"
        });
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
        {
            return Unauthorized(new
            {
                success = false,
                message = "Invalid email or password"
            });
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return Unauthorized(new
            {
                success = false,
                message = "Invalid email or password"
            });
        }
        // التحقق من أن الحساب مقبول ومفعّل من الآدمن
        if (!user.IsApproved)
        {
            return Unauthorized(new
            {
                success = false,
                message = "حسابك قيد المراجعة، يرجى الانتظار لحين قبول طلبك من قِبل المسؤول."
            });
        }

        var token = GenerateToken(user);

        return Ok(new
        {
            success = true,
            token
        });

    }
    // 1. دالة للموافقة على الحساب وتفعيله
    [HttpPut("{id}/approve")]
    public async Task<IActionResult> ApproveUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound(new { success = false, message = "User not found" });
        }

        user.IsApproved = true; // تفعيل الحساب
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "User approved successfully" });
    }

    // 2. دالة لحذف حساب المستخدم نهائياً
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound(new { success = false, message = "User not found" });
        }

        // اختياري: منع الآدمن من حذف نفسه بالخطأ
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "User deleted successfully" });
    }
    private string GenerateToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
       //88
        var claims = new[]
     {
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(ClaimTypes.Role, user.Role)
};
        //454
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}