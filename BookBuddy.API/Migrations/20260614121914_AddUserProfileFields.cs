using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBuddy.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfileFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookss",
                keyColumn: "BookId",
                keyValue: new Guid("52223e56-2c88-4a14-a62e-290322ead0ff"));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisteredAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Bookss",
                columns: new[] { "BookId", "Author", "AvailableCopies", "BookAddedAt", "Category", "ISBN", "IsActive", "Popularity", "Price", "PublishedAt", "Title", "TotalCopies" },
                values: new object[] { new Guid("e0b59cff-9cb2-42bd-9cce-bb6ed9aaae79"), "Dr. APJ Abdul kalam", 7, new DateTime(2026, 6, 14, 17, 49, 14, 234, DateTimeKind.Local).AddTicks(2352), "AutoBiography", "978-0132350884", true, 9, 589m, new DateTime(2000, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wings of Fire", 10 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookss",
                keyColumn: "BookId",
                keyValue: new Guid("e0b59cff-9cb2-42bd-9cce-bb6ed9aaae79"));

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RegisteredAt",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "Bookss",
                columns: new[] { "BookId", "Author", "AvailableCopies", "BookAddedAt", "Category", "ISBN", "IsActive", "Popularity", "Price", "PublishedAt", "Title", "TotalCopies" },
                values: new object[] { new Guid("52223e56-2c88-4a14-a62e-290322ead0ff"), "Dr. APJ Abdul kalam", 7, new DateTime(2026, 6, 14, 14, 22, 10, 96, DateTimeKind.Local).AddTicks(1056), "AutoBiography", "978-0132350884", true, 9, 589m, new DateTime(2000, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wings of Fire", 10 });
        }
    }
}
