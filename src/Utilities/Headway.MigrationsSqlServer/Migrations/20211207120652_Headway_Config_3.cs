using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class Headway_Config_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Column",
                table: "ConfigContainers");

            migrationBuilder.DropColumn(
                name: "Row",
                table: "ConfigContainers");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "ConfigContainers",
                newName: "Label");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Configs",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Configs");

            migrationBuilder.RenameColumn(
                name: "Label",
                table: "ConfigContainers",
                newName: "Text");

            migrationBuilder.AddColumn<int>(
                name: "Column",
                table: "ConfigContainers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Row",
                table: "ConfigContainers",
                type: "int",
                nullable: true);
        }
    }
}
