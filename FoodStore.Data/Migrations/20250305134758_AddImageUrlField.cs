using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStore.Data.Migrations
{
    public partial class AddImageUrlField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "foods");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "foods",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "foods");

            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "foods",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
