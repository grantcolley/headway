using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class Headway_ConfigContainer_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfigContainers_Configs_ConfigId",
                table: "ConfigContainers");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfigItems_Configs_ConfigId",
                table: "ConfigItems");

            migrationBuilder.AlterColumn<int>(
                name: "ConfigId",
                table: "ConfigItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ConfigId",
                table: "ConfigContainers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContainerCode",
                table: "ConfigContainers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentContainerCode",
                table: "ConfigContainers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigContainers_Configs_ConfigId",
                table: "ConfigContainers",
                column: "ConfigId",
                principalTable: "Configs",
                principalColumn: "ConfigId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigItems_Configs_ConfigId",
                table: "ConfigItems",
                column: "ConfigId",
                principalTable: "Configs",
                principalColumn: "ConfigId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfigContainers_Configs_ConfigId",
                table: "ConfigContainers");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfigItems_Configs_ConfigId",
                table: "ConfigItems");

            migrationBuilder.DropColumn(
                name: "ContainerCode",
                table: "ConfigContainers");

            migrationBuilder.DropColumn(
                name: "ParentContainerCode",
                table: "ConfigContainers");

            migrationBuilder.AlterColumn<int>(
                name: "ConfigId",
                table: "ConfigItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ConfigId",
                table: "ConfigContainers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigContainers_Configs_ConfigId",
                table: "ConfigContainers",
                column: "ConfigId",
                principalTable: "Configs",
                principalColumn: "ConfigId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigItems_Configs_ConfigId",
                table: "ConfigItems",
                column: "ConfigId",
                principalTable: "Configs",
                principalColumn: "ConfigId");
        }
    }
}
