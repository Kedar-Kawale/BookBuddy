using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBuddy.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentGatewayFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookss",
                keyColumn: "BookId",
                keyValue: new Guid("e0b59cff-9cb2-42bd-9cce-bb6ed9aaae79"));

            migrationBuilder.AddColumn<string>(
                name: "GatewayOrderId",
                table: "Paymentss",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GatewayPaymentId",
                table: "Paymentss",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Paymentss",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Bookss",
                columns: new[] { "BookId", "Author", "AvailableCopies", "BookAddedAt", "Category", "ISBN", "IsActive", "Popularity", "Price", "PublishedAt", "Title", "TotalCopies" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "Dr. APJ Abdul kalam", 7, new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "AutoBiography", "978-0132350884", true, 9, 589m, new DateTime(2000, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wings of Fire", 10 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookss",
                keyColumn: "BookId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DropColumn(
                name: "GatewayOrderId",
                table: "Paymentss");

            migrationBuilder.DropColumn(
                name: "GatewayPaymentId",
                table: "Paymentss");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Paymentss");

            migrationBuilder.InsertData(
                table: "Bookss",
                columns: new[] { "BookId", "Author", "AvailableCopies", "BookAddedAt", "Category", "ISBN", "IsActive", "Popularity", "Price", "PublishedAt", "Title", "TotalCopies" },
                values: new object[] { new Guid("e0b59cff-9cb2-42bd-9cce-bb6ed9aaae79"), "Dr. APJ Abdul kalam", 7, new DateTime(2026, 6, 14, 17, 49, 14, 234, DateTimeKind.Local).AddTicks(2352), "AutoBiography", "978-0132350884", true, 9, 589m, new DateTime(2000, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wings of Fire", 10 });
        }
    }
}
