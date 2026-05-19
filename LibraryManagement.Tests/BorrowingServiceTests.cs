using FluentAssertions;
using Library_Management_System_API.Services;
using Xunit;

namespace LibraryManagement.Tests.Services
{
    public class BorrowingServiceTests
    {
        private readonly BorrowingService _borrowingService;

        public BorrowingServiceTests()
        {
            _borrowingService = new BorrowingService();
        }

        [Fact]
        public void CanBorrowBook_ShouldReturnTrue_WhenAvailableCopiesGreaterThanZero()
        {
            var result = _borrowingService.CanBorrowBook(3);

            result.Should().BeTrue();
        }

        [Fact]
        public void CanBorrowBook_ShouldReturnFalse_WhenAvailableCopiesIsZero()
        {
            var result = _borrowingService.CanBorrowBook(0);

            result.Should().BeFalse();
        }
    }
}