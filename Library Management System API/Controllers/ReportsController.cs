using Library_Management_System_API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Librarian")]
public class ReportsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReportsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var totalBooks = await _context.Books.CountAsync();

        var totalUsers = await _context.Users.CountAsync();

        var activeBorrowings = await _context.Borrowings
            .CountAsync(b => b.Status == "Borrowed");

        var returnedBorrowings = await _context.Borrowings
            .CountAsync(b => b.Status == "Returned");

        var overdueBorrowings = await _context.Borrowings
            .CountAsync(b =>
                b.Status == "Borrowed" &&
                b.DueDate < DateTime.Now
            );

        return Ok(new
        {
            success = true,
            message = "Summary report retrieved successfully",
            data = new
            {
                totalBooks,
                totalUsers,
                activeBorrowings,
                returnedBorrowings,
                overdueBorrowings
            }
        });
    }
    [HttpGet("most-borrowed")]
    public async Task<IActionResult> GetMostBorrowedBooks()
    {
        var result = await _context.Borrowings
            .GroupBy(b => b.BookId)
            .Select(g => new
            {
                BookId = g.Key,
                BorrowCount = g.Count()
            })
            .OrderByDescending(x => x.BorrowCount)
            .Take(5)
            .Join(_context.Books,
                g => g.BookId,
                b => b.Id,
                (g, b) => new
                {
                    b.Title,
                    b.Author,
                    g.BorrowCount
                })
            .ToListAsync();

        return Ok(new
        {
            success = true,
            data = result
        });
    }
}