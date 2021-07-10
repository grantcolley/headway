using Microsoft.EntityFrameworkCore.Migrations;

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class HeadwayDropBrowserStorageItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrowserStorageItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrowserStorageItems",
                columns: table => new
                {
                    BrowserStorageItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrowserStorageItems", x => x.BrowserStorageItemId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrowserStorageItems_Key",
                table: "BrowserStorageItems",
                column: "Key",
                unique: true,
                filter: "[Key] IS NOT NULL");
        }
    }
}
