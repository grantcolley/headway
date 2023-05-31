using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Headway_State_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContextFullName",
                table: "States",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContextProperties",
                table: "States",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContextFullName",
                table: "States");

            migrationBuilder.DropColumn(
                name: "ContextProperties",
                table: "States");
        }
    }
}
