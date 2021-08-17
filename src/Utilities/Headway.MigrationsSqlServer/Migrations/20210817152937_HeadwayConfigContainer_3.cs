using Microsoft.EntityFrameworkCore.Migrations;

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class HeadwayConfigContainer_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRootContainer",
                table: "ConfigContainers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRootContainer",
                table: "ConfigContainers");
        }
    }
}
