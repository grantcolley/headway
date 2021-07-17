using Microsoft.EntityFrameworkCore.Migrations;

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class HeadwayConfigUpdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ModelConfigs_ModelName",
                table: "ModelConfigs");

            migrationBuilder.RenameColumn(
                name: "ModelName",
                table: "ModelConfigs",
                newName: "Model");

            migrationBuilder.RenameColumn(
                name: "ConfigApiPath",
                table: "ModelConfigs",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ConfigPath",
                table: "ListConfigs",
                newName: "Model");

            migrationBuilder.AddColumn<string>(
                name: "ConfigApi",
                table: "ModelConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConfigApi",
                table: "ListConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModelConfigs_Model",
                table: "ModelConfigs",
                column: "Model",
                unique: true,
                filter: "[Model] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ModelConfigs_Model",
                table: "ModelConfigs");

            migrationBuilder.DropColumn(
                name: "ConfigApi",
                table: "ModelConfigs");

            migrationBuilder.DropColumn(
                name: "ConfigApi",
                table: "ListConfigs");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ModelConfigs",
                newName: "ConfigApiPath");

            migrationBuilder.RenameColumn(
                name: "Model",
                table: "ModelConfigs",
                newName: "ModelName");

            migrationBuilder.RenameColumn(
                name: "Model",
                table: "ListConfigs",
                newName: "ConfigPath");

            migrationBuilder.CreateIndex(
                name: "IX_ModelConfigs_ModelName",
                table: "ModelConfigs",
                column: "ModelName",
                unique: true,
                filter: "[ModelName] IS NOT NULL");
        }
    }
}
