using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_Management_System_API.Migrations
{
    /// <inheritdoc />
    public partial class AddFineAmountToBorrowing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FineAmount",
                table: "Borrowings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FineAmount",
                table: "Borrowings");
        }
    }
}
