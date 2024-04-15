using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiraj_Foods.Migrations
{
    /// <inheritdoc />
    public partial class AddQuantityToCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductsAndQuantity",
                table: "Checkout",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Cart",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductsAndQuantity",
                table: "Checkout");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Cart");
        }
    }
}
