using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class Headway_Demo_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemoModelItems_DemoModels_DemoModelId",
                table: "DemoModelItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DemoModelTreeItems_DemoModels_DemoModelId",
                table: "DemoModelTreeItems");

            migrationBuilder.AlterColumn<int>(
                name: "DemoModelId",
                table: "DemoModelTreeItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemCode",
                table: "DemoModelTreeItems",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParentItemCode",
                table: "DemoModelTreeItems",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DemoModelId",
                table: "DemoModelItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DemoModelItems_DemoModels_DemoModelId",
                table: "DemoModelItems",
                column: "DemoModelId",
                principalTable: "DemoModels",
                principalColumn: "DemoModelId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DemoModelTreeItems_DemoModels_DemoModelId",
                table: "DemoModelTreeItems",
                column: "DemoModelId",
                principalTable: "DemoModels",
                principalColumn: "DemoModelId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemoModelItems_DemoModels_DemoModelId",
                table: "DemoModelItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DemoModelTreeItems_DemoModels_DemoModelId",
                table: "DemoModelTreeItems");

            migrationBuilder.DropColumn(
                name: "ItemCode",
                table: "DemoModelTreeItems");

            migrationBuilder.DropColumn(
                name: "ParentItemCode",
                table: "DemoModelTreeItems");

            migrationBuilder.AlterColumn<int>(
                name: "DemoModelId",
                table: "DemoModelTreeItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DemoModelId",
                table: "DemoModelItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DemoModelItems_DemoModels_DemoModelId",
                table: "DemoModelItems",
                column: "DemoModelId",
                principalTable: "DemoModels",
                principalColumn: "DemoModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_DemoModelTreeItems_DemoModels_DemoModelId",
                table: "DemoModelTreeItems",
                column: "DemoModelId",
                principalTable: "DemoModels",
                principalColumn: "DemoModelId");
        }
    }
}
