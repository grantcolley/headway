using Microsoft.EntityFrameworkCore.Migrations;

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class Headway_LayoutConfig_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LayoutConfigId",
                table: "Configs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LayoutConfigItemsLayoutConfigItemId",
                table: "ConfigItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LayoutConfigs",
                columns: table => new
                {
                    LayoutConfigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LayoutConfigs", x => x.LayoutConfigId);
                });

            migrationBuilder.CreateTable(
                name: "LayoutConfigItems",
                columns: table => new
                {
                    LayoutConfigItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Component = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Row = table.Column<int>(type: "int", nullable: false),
                    Column = table.Column<int>(type: "int", nullable: false),
                    LayoutConfigId = table.Column<int>(type: "int", nullable: true),
                    LayoutConfigItemId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LayoutConfigItems", x => x.LayoutConfigItemId);
                    table.ForeignKey(
                        name: "FK_LayoutConfigItems_LayoutConfigItems_LayoutConfigItemId1",
                        column: x => x.LayoutConfigItemId1,
                        principalTable: "LayoutConfigItems",
                        principalColumn: "LayoutConfigItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LayoutConfigItems_LayoutConfigs_LayoutConfigId",
                        column: x => x.LayoutConfigId,
                        principalTable: "LayoutConfigs",
                        principalColumn: "LayoutConfigId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configs_LayoutConfigId",
                table: "Configs",
                column: "LayoutConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItems_LayoutConfigItemsLayoutConfigItemId",
                table: "ConfigItems",
                column: "LayoutConfigItemsLayoutConfigItemId");

            migrationBuilder.CreateIndex(
                name: "IX_LayoutConfigItems_LayoutConfigId",
                table: "LayoutConfigItems",
                column: "LayoutConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_LayoutConfigItems_LayoutConfigItemId1",
                table: "LayoutConfigItems",
                column: "LayoutConfigItemId1");

            migrationBuilder.CreateIndex(
                name: "IX_LayoutConfigs_Name",
                table: "LayoutConfigs",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigItems_LayoutConfigItems_LayoutConfigItemsLayoutConfigItemId",
                table: "ConfigItems",
                column: "LayoutConfigItemsLayoutConfigItemId",
                principalTable: "LayoutConfigItems",
                principalColumn: "LayoutConfigItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Configs_LayoutConfigs_LayoutConfigId",
                table: "Configs",
                column: "LayoutConfigId",
                principalTable: "LayoutConfigs",
                principalColumn: "LayoutConfigId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfigItems_LayoutConfigItems_LayoutConfigItemsLayoutConfigItemId",
                table: "ConfigItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Configs_LayoutConfigs_LayoutConfigId",
                table: "Configs");

            migrationBuilder.DropTable(
                name: "LayoutConfigItems");

            migrationBuilder.DropTable(
                name: "LayoutConfigs");

            migrationBuilder.DropIndex(
                name: "IX_Configs_LayoutConfigId",
                table: "Configs");

            migrationBuilder.DropIndex(
                name: "IX_ConfigItems_LayoutConfigItemsLayoutConfigItemId",
                table: "ConfigItems");

            migrationBuilder.DropColumn(
                name: "LayoutConfigId",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "LayoutConfigItemsLayoutConfigItemId",
                table: "ConfigItems");
        }
    }
}
