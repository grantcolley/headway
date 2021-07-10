using Microsoft.EntityFrameworkCore.Migrations;

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class HeadwayListConfigUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RedirectText",
                table: "ModelConfigs",
                newName: "NavigateTo");

            migrationBuilder.RenameColumn(
                name: "RedirectPage",
                table: "ModelConfigs",
                newName: "NavigateText");

            migrationBuilder.AddColumn<string>(
                name: "NavigateTo",
                table: "ListConfigs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NavigateTo",
                table: "ListConfigs");

            migrationBuilder.RenameColumn(
                name: "NavigateTo",
                table: "ModelConfigs",
                newName: "RedirectText");

            migrationBuilder.RenameColumn(
                name: "NavigateText",
                table: "ModelConfigs",
                newName: "RedirectPage");
        }
    }
}
