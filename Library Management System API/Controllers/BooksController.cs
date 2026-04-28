using Library_Management_System_API.Data;
using Library_Management_System_API.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System_API.Controllers
{
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
        public async Task<IActionResult> GetBooks()
        {
            var books = await _context.Books.ToListAsync();

            return Ok(new
            {
                success = true,
                message = "Books retrieved successfully",
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