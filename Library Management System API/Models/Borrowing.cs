namespace Library_Management_System_API.Models
{
    public class Borrowing
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        public DateTime BorrowDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public string Status { get; set; } = "Borrowed";
        public decimal FineAmount { get; set; } = 0;
    }
}
