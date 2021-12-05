using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class Headway_Config_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NavigateBack",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "NavigateBackConfig",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "NavigateBackProperty",
                table: "Configs");

            migrationBuilder.RenameColumn(
                name: "Container",
                table: "Configs",
                newName: "Document");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Document",
                table: "Configs",
                newName: "Container");

            migrationBuilder.AddColumn<string>(
                name: "NavigateBack",
                table: "Configs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NavigateBackConfig",
                table: "Configs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NavigateBackProperty",
                table: "Configs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
