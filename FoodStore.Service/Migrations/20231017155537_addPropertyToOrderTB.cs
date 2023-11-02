using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStore.Service.Migrations
{
    public partial class addPropertyToOrderTB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderDetails_orders_OrderId",
                table: "orderDetails");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "orderDetails",
                newName: "orderId");

            migrationBuilder.RenameIndex(
                name: "IX_orderDetails_OrderId",
                table: "orderDetails",
                newName: "IX_orderDetails_orderId");

            migrationBuilder.AlterColumn<int>(
                name: "orderId",
                table: "orderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_orderDetails_orders_orderId",
                table: "orderDetails",
                column: "orderId",
                principalTable: "orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderDetails_orders_orderId",
                table: "orderDetails");

            migrationBuilder.RenameColumn(
                name: "orderId",
                table: "orderDetails",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_orderDetails_orderId",
                table: "orderDetails",
                newName: "IX_orderDetails_OrderId");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "orderDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_orderDetails_orders_OrderId",
                table: "orderDetails",
                column: "OrderId",
                principalTable: "orders",
                principalColumn: "Id");
        }
    }
}
