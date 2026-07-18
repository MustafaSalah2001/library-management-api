using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_Management_System_API.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByToBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Borrowings");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Borrowings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
