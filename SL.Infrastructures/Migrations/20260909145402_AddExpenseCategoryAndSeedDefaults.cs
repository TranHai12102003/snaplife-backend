using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SL.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseCategoryAndSeedDefaults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpenseCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCategories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ExpenseCategories",
                columns: new[] { "Id", "Color", "CreatedBy", "CreatedDate", "DisplayOrder", "Icon", "IsActive", "IsDefault", "Name", "UpdatedBy", "UpdatedDate", "UserId" },
                values: new object[,]
                {
                    { 1L, "#FF9800", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "🍳", true, true, "Ăn sáng", null, null, null },
                    { 2L, "#4CAF50", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "🍱", true, true, "Ăn trưa", null, null, null },
                    { 3L, "#E91E63", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "🍲", true, true, "Ăn tối", null, null, null },
                    { 4L, "#795548", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "☕", true, true, "Cà phê & Trà sữa", null, null, null },
                    { 5L, "#00BCD4", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, "🛒", true, true, "Đi chợ & Siêu thị", null, null, null },
                    { 6L, "#607D8B", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, "🚗", true, true, "Di chuyển & Xăng xe", null, null, null },
                    { 7L, "#9C27B0", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, "🛍️", true, true, "Mua sắm & Khác", null, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ExpenseCategoryId",
                table: "Posts",
                column: "ExpenseCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_ExpenseCategories_ExpenseCategoryId",
                table: "Posts",
                column: "ExpenseCategoryId",
                principalTable: "ExpenseCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_ExpenseCategories_ExpenseCategoryId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "ExpenseCategories");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ExpenseCategoryId",
                table: "Posts");
        }
    }
}
