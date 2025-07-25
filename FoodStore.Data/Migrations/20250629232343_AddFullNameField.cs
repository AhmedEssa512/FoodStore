﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStore.Data.Migrations
{
    public partial class AddFullNameField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "orders");
        }
    }
}
