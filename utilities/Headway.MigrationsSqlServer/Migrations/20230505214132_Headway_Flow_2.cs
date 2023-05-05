using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class HeadwayFlow2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfigureStatesDuringBootstrap",
                table: "Flows");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<bool>(
                name: "ConfigureStatesDuringBootstrap",
                table: "Flows",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
