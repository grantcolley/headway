using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class Headway_Config_CreateLocal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CreateLocal",
                table: "Configs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateLocal",
                table: "Configs");
        }
    }
}
