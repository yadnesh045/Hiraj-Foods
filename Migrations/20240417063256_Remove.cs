using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiraj_Foods.Migrations
{
    /// <inheritdoc />
    public partial class Remove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductImageUrl",
                table: "Uorders");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Uorders",
                newName: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Products",
                table: "Uorders",
                newName: "ProductName");

            migrationBuilder.AddColumn<string>(
                name: "ProductImageUrl",
                table: "Uorders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
