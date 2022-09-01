using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class Headay_Redress_Product_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Redresses_Products_ProductId",
                table: "Redresses");

            migrationBuilder.DropIndex(
                name: "IX_Redresses_ProductId",
                table: "Redresses");

            migrationBuilder.CreateIndex(
                name: "IX_Redresses_ProductId",
                table: "Redresses",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Redresses_Products_ProductId",
                table: "Redresses",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Redresses_Products_ProductId",
                table: "Redresses");

            migrationBuilder.DropIndex(
                name: "IX_Redresses_ProductId",
                table: "Redresses");

            migrationBuilder.CreateIndex(
                name: "IX_Redresses_ProductId",
                table: "Redresses",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Redresses_Products_ProductId",
                table: "Redresses",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }
    }
}
