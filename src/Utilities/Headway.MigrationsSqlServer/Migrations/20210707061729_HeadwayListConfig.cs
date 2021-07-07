using Microsoft.EntityFrameworkCore.Migrations;

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class HeadwayListConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "ListConfigs",
                columns: table => new
                {
                    ListConfigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ConfigPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdPropertyName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListConfigs", x => x.ListConfigId);
                });

            migrationBuilder.CreateTable(
                name: "ListItemConfigs",
                columns: table => new
                {
                    ListItemConfigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeaderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ListConfigId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListItemConfigs", x => x.ListItemConfigId);
                    table.ForeignKey(
                        name: "FK_ListItemConfigs_ListConfigs_ListConfigId",
                        column: x => x.ListConfigId,
                        principalTable: "ListConfigs",
                        principalColumn: "ListConfigId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrowserStorageItems_Key",
                table: "BrowserStorageItems",
                column: "Key",
                unique: true,
                filter: "[Key] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ListConfigs_ListName",
                table: "ListConfigs",
                column: "ListName",
                unique: true,
                filter: "[ListName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ListItemConfigs_ListConfigId",
                table: "ListItemConfigs",
                column: "ListConfigId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrowserStorageItems");

            migrationBuilder.DropTable(
                name: "ListItemConfigs");

            migrationBuilder.DropTable(
                name: "ListConfigs");
        }
    }
}
