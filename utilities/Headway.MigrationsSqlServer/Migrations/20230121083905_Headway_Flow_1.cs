using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class HeadwayFlow1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActionConfigurationClass",
                table: "States",
                newName: "StateConfigurationClass");

            migrationBuilder.RenameColumn(
                name: "ActionConfigurationClass",
                table: "Flows",
                newName: "FlowConfigurationClass");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StateConfigurationClass",
                table: "States",
                newName: "ActionConfigurationClass");

            migrationBuilder.RenameColumn(
                name: "FlowConfigurationClass",
                table: "Flows",
                newName: "ActionConfigurationClass");
        }
    }
}
