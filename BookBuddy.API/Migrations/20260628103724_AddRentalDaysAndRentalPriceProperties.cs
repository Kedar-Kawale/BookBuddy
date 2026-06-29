using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBuddy.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalDaysAndRentalPriceProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RentalDays",
                table: "Rentalss",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "RentalPrice",
                table: "Rentalss",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentalDays",
                table: "Rentalss");

            migrationBuilder.DropColumn(
                name: "RentalPrice",
                table: "Rentalss");
        }
    }
}
