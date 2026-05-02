using Library_Management_System_API.Data;
using Library_Management_System_API.Dto;
using Library_Management_System_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Library_Management_System_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BorrowingsController : ControllerBase
{
    private readonly AppDbContext _context;

    public BorrowingsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("borrow")]
    [Authorize(Roles = "Member,Admin,Librarian")]
    public async Task<IActionResult> BorrowBook(BorrowBookDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var book = await _context.Books.FindAsync(dto.BookId);

        if (book == null)
        {
            return NotFound(new
            {
                success = false,
                message = "Book not found"
            });
        }

        if (book.Quantity <= 0)
        {
            return BadRequest(new
            {
                success = false,
                message = "Book is not available"
            });
        }

        var alreadyBorrowed = await _context.Borrowings.AnyAsync(b =>
            b.UserId == userId &&
            b.BookId == dto.BookId &&
            b.Status == "Borrowed"
        );

        if (alreadyBorrowed)
        {
            return BadRequest(new
            {
                success = false,
                message = "You already borrowed this book"
            });
        }

        var borrowing = new Borrowing
        {
            UserId = userId,
            BookId = dto.BookId,
            BorrowDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(14),
            Status = "Borrowed"
        };

        book.Quantity--;

        _context.Borrowings.Add(borrowing);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "Book borrowed successfully",
            data = borrowing
        });
    }

    [HttpPut("{id}/return")]
    [Authorize(Roles = "Member,Admin,Librarian")]
    public async Task<IActionResult> ReturnBook(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role);

        var borrowing = await _context.Borrowings
            .Include(b => b.Book)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (borrowing == null)
        {
            return NotFound(new
            {
                success = false,
                message = "Borrowing record not found"
            });
        }

        if (role == "Member" && borrowing.UserId != userId)
        {
            return Forbid();
        }

        if (borrowing.Status == "Returned")
        {
            return BadRequest(new
            {
                success = false,
                message = "Book already returned"
            });
        }

        // حساب الغرامة إذا تأخر
        if (DateTime.Now > borrowing.DueDate)
        {
            var daysLate = (DateTime.Now - borrowing.DueDate).Days;
            borrowing.FineAmount = daysLate * 1;
        }

        // إرجاع الكتاب
        borrowing.ReturnDate = DateTime.Now;
        borrowing.Status = "Returned";
        borrowing.Book.Quantity++;
        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "Book returned successfully",
            data = borrowing
        });
    }

    [HttpGet("my")]
    [Authorize(Roles = "Member,Admin,Librarian")]
    public async Task<IActionResult> GetMyBorrowings()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var borrowings = await _context.Borrowings
            .Include(b => b.Book)
            .Where(b => b.UserId == userId)
           .Select(b => new
           {
               b.Id,
               b.BookId,
               BookTitle = b.Book.Title,
               b.BorrowDate,
               b.DueDate,
               b.ReturnDate,
               b.Status,
               b.FineAmount
           })
            .ToListAsync();

        return Ok(new
        {
            success = true,
            data = borrowings
        });
    }
    [HttpGet("overdue")]
    [Authorize(Roles = "Admin,Librarian")]
    public async Task<IActionResult> GetOverdueBorrowings()
    {
        var overdueBorrowings = await _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.User)
            .Where(b =>
                b.Status == "Borrowed" &&
                b.DueDate < DateTime.Now
            )
            .Select(b => new
            {
                b.Id,
                UserId = b.UserId,
                Username = b.User.Username,
                BookId = b.BookId,
                BookTitle = b.Book.Title,
                b.BorrowDate,
                b.DueDate,
                DaysLate = (DateTime.Now - b.DueDate).Days,
                EstimatedFine = (DateTime.Now - b.DueDate).Days * 1
            })
            .ToListAsync();

        return Ok(new
        {
            success = true,
            message = "Overdue borrowings retrieved successfully",
            data = overdueBorrowings
        });
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Librarian")]
    public async Task<IActionResult> GetAllBorrowings()
    {
        var borrowings = await _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.User)
            .Select(b => new
            {
                b.Id,
                Username = b.User.Username,
                BookTitle = b.Book.Title,
                b.BorrowDate,
                b.DueDate,
                b.ReturnDate,
                b.Status
            })
            .ToListAsync();

        return Ok(new
        {
            success = true,
            data = borrowings
        });
    }
}