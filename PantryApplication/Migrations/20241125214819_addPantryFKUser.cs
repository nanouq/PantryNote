using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PantryApplication.Migrations
{
    /// <inheritdoc />
    public partial class addPantryFKUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Pantry",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Pantry",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.UpdateData(
                table: "Pantry",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId",
                value: "39d77c7b-2212-4ded-85cf-9f86cb9d18df");

            migrationBuilder.CreateIndex(
                name: "IX_Pantry_UserId",
                table: "Pantry",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pantry_AspNetUsers_UserId",
                table: "Pantry",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pantry_AspNetUsers_UserId",
                table: "Pantry");

            migrationBuilder.DropIndex(
                name: "IX_Pantry_UserId",
                table: "Pantry");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Pantry",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "Item",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdded", "ExpirationDate" },
                values: new object[] { new DateTime(2024, 11, 23, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Item",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateAdded", "ExpirationDate" },
                values: new object[] { new DateTime(2024, 11, 23, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 11, 23, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Pantry",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId",
                value: 1);

            migrationBuilder.InsertData(
                table: "Pantry",
                columns: new[] { "Id", "UserId" },
                values: new object[] { 2, 2 });
        }
    }
}
