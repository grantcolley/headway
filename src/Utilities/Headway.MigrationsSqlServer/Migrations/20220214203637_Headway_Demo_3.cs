using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class Headway_Demo_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Dropdown",
                table: "DemoModels",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dropdown",
                table: "DemoModels");
        }
    }
}
