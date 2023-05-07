using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class HeadwayFlow3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Permissions",
                table: "States",
                newName: "WritePermission");

            migrationBuilder.AddColumn<string>(
                name: "ReadPermission",
                table: "States",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadPermission",
                table: "States");

            migrationBuilder.RenameColumn(
                name: "WritePermission",
                table: "States",
                newName: "Permissions");
        }
    }
}
