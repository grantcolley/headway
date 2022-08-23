using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class Headway_Redress_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Redresses_Customers_CustomerId",
                table: "Redresses");

            migrationBuilder.DropIndex(
                name: "IX_Redresses_CustomerId",
                table: "Redresses");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Redresses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Redresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Redresses_CustomerId",
                table: "Redresses",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Redresses_Customers_CustomerId",
                table: "Redresses",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
