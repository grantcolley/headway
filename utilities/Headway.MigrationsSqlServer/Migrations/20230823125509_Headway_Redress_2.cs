using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Headway_Redress_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RedressFlowContexts_Redresses_RedressId",
                table: "RedressFlowContexts");

            migrationBuilder.DropIndex(
                name: "IX_RedressFlowContexts_RedressId",
                table: "RedressFlowContexts");

            migrationBuilder.RenameColumn(
                name: "RefressFlowContextId",
                table: "Redresses",
                newName: "RedressFlowContextId");

            migrationBuilder.AddColumn<int>(
                name: "RedressFlowContextId1",
                table: "Redresses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Redresses_RedressFlowContextId1",
                table: "Redresses",
                column: "RedressFlowContextId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Redresses_RedressFlowContexts_RedressFlowContextId1",
                table: "Redresses",
                column: "RedressFlowContextId1",
                principalTable: "RedressFlowContexts",
                principalColumn: "RedressFlowContextId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Redresses_RedressFlowContexts_RedressFlowContextId1",
                table: "Redresses");

            migrationBuilder.DropIndex(
                name: "IX_Redresses_RedressFlowContextId1",
                table: "Redresses");

            migrationBuilder.DropColumn(
                name: "RedressFlowContextId1",
                table: "Redresses");

            migrationBuilder.RenameColumn(
                name: "RedressFlowContextId",
                table: "Redresses",
                newName: "RefressFlowContextId");

            migrationBuilder.CreateIndex(
                name: "IX_RedressFlowContexts_RedressId",
                table: "RedressFlowContexts",
                column: "RedressId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RedressFlowContexts_Redresses_RedressId",
                table: "RedressFlowContexts",
                column: "RedressId",
                principalTable: "Redresses",
                principalColumn: "RedressId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
