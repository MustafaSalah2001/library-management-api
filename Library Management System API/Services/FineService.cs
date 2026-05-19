namespace Library_Management_System_API.Services
{
    public class FineService
    {
        public decimal CalculateFine(int daysLate, decimal finePerDay)
        {
            if (daysLate <= 0)
                return 0;

            return daysLate * finePerDay;
        }
    }
}