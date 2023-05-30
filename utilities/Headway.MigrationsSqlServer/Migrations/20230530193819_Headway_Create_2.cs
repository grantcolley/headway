using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Headway_Create_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RedressFlowContexts_Users_CurrentUserUserId",
                table: "RedressFlowContexts");

            migrationBuilder.DropIndex(
                name: "IX_RedressFlowContexts_CurrentUserUserId",
                table: "RedressFlowContexts");

            migrationBuilder.DropColumn(
                name: "CurrentUserUserId",
                table: "RedressFlowContexts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentUserUserId",
                table: "RedressFlowContexts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RedressFlowContexts_CurrentUserUserId",
                table: "RedressFlowContexts",
                column: "CurrentUserUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RedressFlowContexts_Users_CurrentUserUserId",
                table: "RedressFlowContexts",
                column: "CurrentUserUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
