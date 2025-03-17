using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStore.Data.Migrations
{
    public partial class CartItemsTB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_carts_foods_foodId",
                table: "carts");

            migrationBuilder.DropIndex(
                name: "IX_carts_foodId",
                table: "carts");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "carts");

            migrationBuilder.DropColumn(
                name: "foodId",
                table: "carts");

            migrationBuilder.AddColumn<double>(
                name: "Total",
                table: "carts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "cartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    FoodId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cartItems_carts_CartId",
                        column: x => x.CartId,
                        principalTable: "carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cartItems_foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cartItems_CartId",
                table: "cartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_cartItems_FoodId",
                table: "cartItems",
                column: "FoodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cartItems");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "carts");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "foodId",
                table: "carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_carts_foodId",
                table: "carts",
                column: "foodId");

            migrationBuilder.AddForeignKey(
                name: "FK_carts_foods_foodId",
                table: "carts",
                column: "foodId",
                principalTable: "foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
