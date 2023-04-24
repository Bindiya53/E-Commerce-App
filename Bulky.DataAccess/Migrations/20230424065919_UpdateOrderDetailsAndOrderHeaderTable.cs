using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bulky.DataAccess.Migrations
{
    public partial class UpdateOrderDetailsAndOrderHeaderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderDetails_Products_OrderHeaderId",
                table: "orderDetails");

            migrationBuilder.CreateIndex(
                name: "IX_orderDetails_ProductId",
                table: "orderDetails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_orderDetails_Products_ProductId",
                table: "orderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderDetails_Products_ProductId",
                table: "orderDetails");

            migrationBuilder.DropIndex(
                name: "IX_orderDetails_ProductId",
                table: "orderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_orderDetails_Products_OrderHeaderId",
                table: "orderDetails",
                column: "OrderHeaderId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
