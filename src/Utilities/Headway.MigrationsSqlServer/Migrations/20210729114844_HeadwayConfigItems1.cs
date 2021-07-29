using Microsoft.EntityFrameworkCore.Migrations;

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class HeadwayConfigItems1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NavigateToPropertyName",
                table: "Configs",
                newName: "NavigateToProperty");

            migrationBuilder.AddColumn<string>(
                name: "NavigateBackProperty",
                table: "Configs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NavigateBackProperty",
                table: "Configs");

            migrationBuilder.RenameColumn(
                name: "NavigateToProperty",
                table: "Configs",
                newName: "NavigateToPropertyName");
        }
    }
}
