using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PantryApplication.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Item",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdded", "ExpirationDate" },
                values: new object[] { new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Item",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateAdded", "ExpirationDate" },
                values: new object[] { new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 12, 7, 0, 0, 0, 0, DateTimeKind.Local) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Item",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdded", "ExpirationDate" },
                values: new object[] { new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Item",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateAdded", "ExpirationDate" },
                values: new object[] { new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 11, 25, 0, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
