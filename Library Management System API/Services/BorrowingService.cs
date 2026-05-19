namespace Library_Management_System_API.Services
{
    public class BorrowingService
    {
        public bool CanBorrowBook(int availableCopies)
        {
            return availableCopies > 0;
        }
    }
}