using Microsoft.EntityFrameworkCore.Migrations;

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class HeadwayConfitContainer_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsContainer",
                table: "ConfigItems");

            migrationBuilder.AddColumn<int>(
                name: "ConfigContainerId",
                table: "ConfigItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConfigContainers",
                columns: table => new
                {
                    ConfigContainerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Container = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Row = table.Column<int>(type: "int", nullable: false),
                    Column = table.Column<int>(type: "int", nullable: false),
                    ConfigContainerId1 = table.Column<int>(type: "int", nullable: true),
                    ConfigId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigContainers", x => x.ConfigContainerId);
                    table.ForeignKey(
                        name: "FK_ConfigContainers_ConfigContainers_ConfigContainerId1",
                        column: x => x.ConfigContainerId1,
                        principalTable: "ConfigContainers",
                        principalColumn: "ConfigContainerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConfigContainers_Configs_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "Configs",
                        principalColumn: "ConfigId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItems_ConfigContainerId",
                table: "ConfigItems",
                column: "ConfigContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigContainers_ConfigContainerId1",
                table: "ConfigContainers",
                column: "ConfigContainerId1");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigContainers_ConfigId",
                table: "ConfigContainers",
                column: "ConfigId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigItems_ConfigContainers_ConfigContainerId",
                table: "ConfigItems",
                column: "ConfigContainerId",
                principalTable: "ConfigContainers",
                principalColumn: "ConfigContainerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfigItems_ConfigContainers_ConfigContainerId",
                table: "ConfigItems");

            migrationBuilder.DropTable(
                name: "ConfigContainers");

            migrationBuilder.DropIndex(
                name: "IX_ConfigItems_ConfigContainerId",
                table: "ConfigItems");

            migrationBuilder.DropColumn(
                name: "ConfigContainerId",
                table: "ConfigItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsContainer",
                table: "ConfigItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
