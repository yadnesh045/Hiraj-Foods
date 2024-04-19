using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiraj_Foods.Migrations
{
    /// <inheritdoc />
    public partial class Initial19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Enquiries",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Enquiries");
        }
    }
}
