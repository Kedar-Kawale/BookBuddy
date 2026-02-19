using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBuddy.API.Migrations
{
    /// <inheritdoc />
    public partial class firstbookseededfortesting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Bookss",
                columns: new[] { "BookId", "Author", "AvailableCopies", "BookAddedAt", "Category", "ISBN", "IsActive", "Popularity", "Price", "PublishedAt", "Title", "TotalCopies" },
                values: new object[] { new Guid("88054a96-5b8a-41c7-813d-715c82d5d4b8"), "Dr. APJ Abdul kalam", 7, new DateTime(2026, 2, 19, 20, 8, 6, 453, DateTimeKind.Local).AddTicks(6242), "AutoBiography", "978-0132350884", true, 9, 589m, new DateTime(2000, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wings of Fire", 10 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookss",
                keyColumn: "BookId",
                keyValue: new Guid("88054a96-5b8a-41c7-813d-715c82d5d4b8"));
        }
    }
}
