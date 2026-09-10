using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SL.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseAndGroupFieldsToPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Posts",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "ExpenseCategoryId",
                table: "Posts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FoodName",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GroupId",
                table: "Posts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExpense",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowAmountToFriends",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ExpenseCategoryId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "FoodName",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IsExpense",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ShowAmountToFriends",
                table: "Posts");
        }
    }
}
