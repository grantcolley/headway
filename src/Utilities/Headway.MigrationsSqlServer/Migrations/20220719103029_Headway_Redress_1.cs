using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class Headway_Redress_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RedressProducts");

            migrationBuilder.RenameColumn(
                name: "ResponseRecieved",
                table: "Redresses",
                newName: "ResponseReceived");

            migrationBuilder.RenameColumn(
                name: "RedressLoadDate",
                table: "Redresses",
                newName: "RedressCreateDate");

            migrationBuilder.RenameColumn(
                name: "RedressLoadBy",
                table: "Redresses",
                newName: "RedressCreateBy");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Redresses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Redresses_ProductId",
                table: "Redresses",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Redresses_Products_ProductId",
                table: "Redresses",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Redresses_Products_ProductId",
                table: "Redresses");

            migrationBuilder.DropIndex(
                name: "IX_Redresses_ProductId",
                table: "Redresses");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Redresses");

            migrationBuilder.RenameColumn(
                name: "ResponseReceived",
                table: "Redresses",
                newName: "ResponseRecieved");

            migrationBuilder.RenameColumn(
                name: "RedressCreateDate",
                table: "Redresses",
                newName: "RedressLoadDate");

            migrationBuilder.RenameColumn(
                name: "RedressCreateBy",
                table: "Redresses",
                newName: "RedressLoadBy");

            migrationBuilder.CreateTable(
                name: "RedressProducts",
                columns: table => new
                {
                    RedressProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    RedressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedressProducts", x => x.RedressProductId);
                    table.ForeignKey(
                        name: "FK_RedressProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RedressProducts_Redresses_RedressId",
                        column: x => x.RedressId,
                        principalTable: "Redresses",
                        principalColumn: "RedressId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RedressProducts_ProductId",
                table: "RedressProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RedressProducts_RedressId",
                table: "RedressProducts",
                column: "RedressId");
        }
    }
}
