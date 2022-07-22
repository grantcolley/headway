using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class Headway_Config_SearchItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigSearchItems",
                columns: table => new
                {
                    ConfigSearchItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tooltip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Component = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ConfigId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigSearchItems", x => x.ConfigSearchItemId);
                    table.ForeignKey(
                        name: "FK_ConfigSearchItems_Configs_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "Configs",
                        principalColumn: "ConfigId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigSearchItems_ConfigId",
                table: "ConfigSearchItems",
                column: "ConfigId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigSearchItems");
        }
    }
}
