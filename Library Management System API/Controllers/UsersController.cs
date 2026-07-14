using Library_Management_System_API.Data;
using Library_Management_System_API.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPut("{id}/role")]
    public async Task<IActionResult> UpdateUserRole(int id, UpdateUserRoleDto dto)
    {
        var allowedRoles = new[] { "Admin", "Librarian", "Member" };

        if (!allowedRoles.Contains(dto.Role))
        {
            return BadRequest(new
            {
                success = false,
                message = "Invalid role. Allowed roles: Admin, Librarian, Member"
            });
        }

        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound(new
            {
                success = false,
                message = "User not found"
            });
        }

        user.Role = dto.Role;
        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "User role updated successfully",
            data = new
            {
                user.Id,
                user.Username,
                user.Email,
                user.Role
            }
        });
    }
    [HttpPut("{id}/approve")]
    public async Task<IActionResult> ApproveUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound(new
            {
                success = false,
                message = "User not found"
            });
        }

        user.IsApproved = true; // تحويل الحالة لمقبول
        await _context.SaveChangesAsync(); // حفظ التغيير بقاعدة البيانات

        return Ok(new
        {
            success = true,
            message = "User approved successfully"
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _context.Users
            .Select(u => new
            {
                u.Id,
                u.Username,
                u.Email,
                u.Role
            })
            .ToListAsync();

        return Ok(new
        {
            success = true,
            data = users
        });
    }
}