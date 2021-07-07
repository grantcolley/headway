using Microsoft.EntityFrameworkCore.Migrations;

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class HeadwayListConfigUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ListConfigs_ListName",
                table: "ListConfigs");

            migrationBuilder.RenameColumn(
                name: "ListName",
                table: "ListConfigs",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ListConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ListConfigs_Name",
                table: "ListConfigs",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ListConfigs_Name",
                table: "ListConfigs");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ListConfigs");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ListConfigs",
                newName: "ListName");

            migrationBuilder.CreateIndex(
                name: "IX_ListConfigs_ListName",
                table: "ListConfigs",
                column: "ListName",
                unique: true,
                filter: "[ListName] IS NOT NULL");
        }
    }
}
