using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class Headway_ConfigContainer_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRootContainer",
                table: "ConfigContainers");

            migrationBuilder.RenameColumn(
                name: "ParentItemCode",
                table: "DemoModelTreeItems",
                newName: "ParentCode");

            migrationBuilder.RenameColumn(
                name: "ItemCode",
                table: "DemoModelTreeItems",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "ParentContainerCode",
                table: "ConfigContainers",
                newName: "ParentCode");

            migrationBuilder.RenameColumn(
                name: "ContainerCode",
                table: "ConfigContainers",
                newName: "Code");

            migrationBuilder.AddColumn<string>(
                name: "ComponentArgs",
                table: "ConfigContainers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComponentArgs",
                table: "ConfigContainers");

            migrationBuilder.RenameColumn(
                name: "ParentCode",
                table: "DemoModelTreeItems",
                newName: "ParentItemCode");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "DemoModelTreeItems",
                newName: "ItemCode");

            migrationBuilder.RenameColumn(
                name: "ParentCode",
                table: "ConfigContainers",
                newName: "ParentContainerCode");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "ConfigContainers",
                newName: "ContainerCode");

            migrationBuilder.AddColumn<bool>(
                name: "IsRootContainer",
                table: "ConfigContainers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
