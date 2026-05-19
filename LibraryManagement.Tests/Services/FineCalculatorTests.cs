using Xunit;
using FluentAssertions;
using Library_Management_System_API.Services;

namespace LibraryManagement.Tests.Services
{
    public class FineCalculatorTests
    {
        private readonly FineService _fineService;

        public FineCalculatorTests()
        {
            _fineService = new FineService();
        }

        [Fact]
        public void CalculateFine_ShouldReturnZero_WhenDaysLateIsZero()
        {
            // Arrange
            int daysLate = 0;
            decimal finePerDay = 2;

            // Act
            decimal result = _fineService.CalculateFine(daysLate, finePerDay);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void CalculateFine_ShouldReturnCorrectFine_WhenBookIsLate()
        {
            // Arrange
            int daysLate = 5;
            decimal finePerDay = 2;

            // Act
            decimal result = _fineService.CalculateFine(daysLate, finePerDay);

            // Assert
            result.Should().Be(10);
        }
    }
}