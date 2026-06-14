using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBuddy.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOldUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Userss");

            migrationBuilder.DeleteData(
                table: "Bookss",
                keyColumn: "BookId",
                keyValue: new Guid("75860764-1497-407b-97e7-e93cd6a3a6f4"));

            migrationBuilder.InsertData(
                table: "Bookss",
                columns: new[] { "BookId", "Author", "AvailableCopies", "BookAddedAt", "Category", "ISBN", "IsActive", "Popularity", "Price", "PublishedAt", "Title", "TotalCopies" },
                values: new object[] { new Guid("52223e56-2c88-4a14-a62e-290322ead0ff"), "Dr. APJ Abdul kalam", 7, new DateTime(2026, 6, 14, 14, 22, 10, 96, DateTimeKind.Local).AddTicks(1056), "AutoBiography", "978-0132350884", true, 9, 589m, new DateTime(2000, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wings of Fire", 10 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookss",
                keyColumn: "BookId",
                keyValue: new Guid("52223e56-2c88-4a14-a62e-290322ead0ff"));

            migrationBuilder.CreateTable(
                name: "Userss",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PinCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Userss", x => x.UserId);
                });

            migrationBuilder.InsertData(
                table: "Bookss",
                columns: new[] { "BookId", "Author", "AvailableCopies", "BookAddedAt", "Category", "ISBN", "IsActive", "Popularity", "Price", "PublishedAt", "Title", "TotalCopies" },
                values: new object[] { new Guid("75860764-1497-407b-97e7-e93cd6a3a6f4"), "Dr. APJ Abdul kalam", 7, new DateTime(2026, 6, 14, 13, 59, 16, 472, DateTimeKind.Local).AddTicks(9419), "AutoBiography", "978-0132350884", true, 9, 589m, new DateTime(2000, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wings of Fire", 10 });
        }
    }
}
