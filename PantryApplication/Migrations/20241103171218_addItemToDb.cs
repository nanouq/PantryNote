using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PantryApplication.Migrations
{
    /// <inheritdoc />
    public partial class addItemToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PantryId = table.Column<int>(type: "int", nullable: false),
                    ItemType = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    IsSealed = table.Column<bool>(type: "bit", nullable: true),
                    RequiresRefrigeration = table.Column<bool>(type: "bit", nullable: true),
                    RequiresFreezing = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_Pantry_PantryId",
                        column: x => x.PantryId,
                        principalTable: "Pantry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Item",
                columns: new[] { "Id", "Brand", "Category", "DateAdded", "ExpirationDate", "ItemType", "Location", "Name", "PantryId", "Quantity", "RequiresFreezing", "RequiresRefrigeration", "Unit" },
                values: new object[] { 1, "Fairlife", "Dairy", new DateTime(2024, 11, 3, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2024, 11, 8, 0, 0, 0, 0, DateTimeKind.Local), "Perishable", "Fridge", "Milk", 1, 2m, false, true, "Liters" });

            migrationBuilder.InsertData(
                table: "Item",
                columns: new[] { "Id", "Brand", "Category", "DateAdded", "ExpirationDate", "IsSealed", "ItemType", "Location", "Name", "PantryId", "Quantity", "Unit" },
                values: new object[] { 2, "Buschs", "Canned Goods", new DateTime(2024, 11, 3, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 11, 3, 0, 0, 0, 0, DateTimeKind.Local), true, "NonPerishable", "Pantry", "Baked Beans", 1, 3m, "Cans" });

            migrationBuilder.CreateIndex(
                name: "IX_Item_PantryId",
                table: "Item",
                column: "PantryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Item");
        }
    }
}
