using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class Headway_Config_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NavigateToProperty",
                table: "Configs",
                newName: "NavigateProperty");

            migrationBuilder.RenameColumn(
                name: "NavigateToConfig",
                table: "Configs",
                newName: "NavigatePage");

            migrationBuilder.RenameColumn(
                name: "NavigateTo",
                table: "Configs",
                newName: "NavigateConfig");

            migrationBuilder.AddColumn<bool>(
                name: "NavigateResetBreadcrumb",
                table: "Configs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NavigateResetBreadcrumb",
                table: "Configs");

            migrationBuilder.RenameColumn(
                name: "NavigateProperty",
                table: "Configs",
                newName: "NavigateToProperty");

            migrationBuilder.RenameColumn(
                name: "NavigatePage",
                table: "Configs",
                newName: "NavigateToConfig");

            migrationBuilder.RenameColumn(
                name: "NavigateConfig",
                table: "Configs",
                newName: "NavigateTo");
        }
    }
}
