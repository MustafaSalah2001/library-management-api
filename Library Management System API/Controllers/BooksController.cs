using Library_Management_System_API.Data;
using Library_Management_System_API.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BooksController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetBooks(
                 int pageNumber = 1,
                 int pageSize = 5,
                 string? search = null,
                 string? category = null,
                 string? author = null)
        {
            pageSize = pageSize > 10 ? 10 : pageSize;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;

            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(b => b.Title.Contains(search));

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(b => b.Category.Contains(category));

            if (!string.IsNullOrWhiteSpace(author))
                query = query.Where(b => b.Author.Contains(author));

            var totalCount = await query.CountAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var books = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Category = b.Category
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                message = "Books retrieved successfully",
                pagination = new
                {
                    pageNumber,
                    pageSize,
                    totalCount,
                    totalPages,
                    hasNextPage = pageNumber < totalPages,
                    hasPreviousPage = pageNumber > 1
                },
                data = books
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Book not found"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Book retrieved successfully",
                data = book
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddBook(CreateBookDto dto)
        {
            var book = new Models.Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Category = dto.Category,
                ISBN = dto.ISBN,
                Quantity = dto.Quantity
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, new
            {
                success = true,
                message = "Book created successfully",
                data = book
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, UpdateBookDto dto)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Book not found"
                });
            }

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.Category = dto.Category;
            book.ISBN = dto.ISBN;
            book.Quantity = dto.Quantity;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Book updated successfully",
                data = book
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Book not found"
                });
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Book deleted successfully"
            });
        }
    }
}