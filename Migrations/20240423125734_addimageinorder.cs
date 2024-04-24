﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiraj_Foods.Migrations
{
    /// <inheritdoc />
    public partial class addimageinorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductImageUrl",
                table: "Uorders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductImageUrl",
                table: "Uorders");
        }
    }
}
