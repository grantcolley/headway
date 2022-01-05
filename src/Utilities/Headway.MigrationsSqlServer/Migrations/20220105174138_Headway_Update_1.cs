using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class Headway_Update_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemoModelItem_DemoModels_DemoModelId",
                table: "DemoModelItem");

            migrationBuilder.DropForeignKey(
                name: "FK_DemoModelTreeItem_DemoModels_DemoModelId",
                table: "DemoModelTreeItem");

            migrationBuilder.DropForeignKey(
                name: "FK_DemoModelTreeItem_DemoModelTreeItem_DemoModelTreeItemId1",
                table: "DemoModelTreeItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DemoModelTreeItem",
                table: "DemoModelTreeItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DemoModelItem",
                table: "DemoModelItem");

            migrationBuilder.RenameTable(
                name: "DemoModelTreeItem",
                newName: "DemoModelTreeItems");

            migrationBuilder.RenameTable(
                name: "DemoModelItem",
                newName: "DemoModelItems");

            migrationBuilder.RenameIndex(
                name: "IX_DemoModelTreeItem_DemoModelTreeItemId1",
                table: "DemoModelTreeItems",
                newName: "IX_DemoModelTreeItems_DemoModelTreeItemId1");

            migrationBuilder.RenameIndex(
                name: "IX_DemoModelTreeItem_DemoModelId",
                table: "DemoModelTreeItems",
                newName: "IX_DemoModelTreeItems_DemoModelId");

            migrationBuilder.RenameIndex(
                name: "IX_DemoModelItem_DemoModelId",
                table: "DemoModelItems",
                newName: "IX_DemoModelItems_DemoModelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DemoModelTreeItems",
                table: "DemoModelTreeItems",
                column: "DemoModelTreeItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DemoModelItems",
                table: "DemoModelItems",
                column: "DemoModelItemId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_DemoModelTreeItems_DemoModelTreeItems_DemoModelTreeItemId1",
                table: "DemoModelTreeItems",
                column: "DemoModelTreeItemId1",
                principalTable: "DemoModelTreeItems",
                principalColumn: "DemoModelTreeItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemoModelItems_DemoModels_DemoModelId",
                table: "DemoModelItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DemoModelTreeItems_DemoModels_DemoModelId",
                table: "DemoModelTreeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DemoModelTreeItems_DemoModelTreeItems_DemoModelTreeItemId1",
                table: "DemoModelTreeItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DemoModelTreeItems",
                table: "DemoModelTreeItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DemoModelItems",
                table: "DemoModelItems");

            migrationBuilder.RenameTable(
                name: "DemoModelTreeItems",
                newName: "DemoModelTreeItem");

            migrationBuilder.RenameTable(
                name: "DemoModelItems",
                newName: "DemoModelItem");

            migrationBuilder.RenameIndex(
                name: "IX_DemoModelTreeItems_DemoModelTreeItemId1",
                table: "DemoModelTreeItem",
                newName: "IX_DemoModelTreeItem_DemoModelTreeItemId1");

            migrationBuilder.RenameIndex(
                name: "IX_DemoModelTreeItems_DemoModelId",
                table: "DemoModelTreeItem",
                newName: "IX_DemoModelTreeItem_DemoModelId");

            migrationBuilder.RenameIndex(
                name: "IX_DemoModelItems_DemoModelId",
                table: "DemoModelItem",
                newName: "IX_DemoModelItem_DemoModelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DemoModelTreeItem",
                table: "DemoModelTreeItem",
                column: "DemoModelTreeItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DemoModelItem",
                table: "DemoModelItem",
                column: "DemoModelItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_DemoModelItem_DemoModels_DemoModelId",
                table: "DemoModelItem",
                column: "DemoModelId",
                principalTable: "DemoModels",
                principalColumn: "DemoModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_DemoModelTreeItem_DemoModels_DemoModelId",
                table: "DemoModelTreeItem",
                column: "DemoModelId",
                principalTable: "DemoModels",
                principalColumn: "DemoModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_DemoModelTreeItem_DemoModelTreeItem_DemoModelTreeItemId1",
                table: "DemoModelTreeItem",
                column: "DemoModelTreeItemId1",
                principalTable: "DemoModelTreeItem",
                principalColumn: "DemoModelTreeItemId");
        }
    }
}
