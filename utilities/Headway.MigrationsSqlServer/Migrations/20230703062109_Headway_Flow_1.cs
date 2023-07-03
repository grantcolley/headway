using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Headway_Flow_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "RedressFlowHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "RedressFlowHistory");
        }
    }
}
