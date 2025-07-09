using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_IsActive_in_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Transactions");
        }
    }
}
