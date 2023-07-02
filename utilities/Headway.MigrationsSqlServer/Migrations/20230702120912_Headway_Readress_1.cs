using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Headway_Readress_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Redresses_Programs_ProgramId",
                table: "Redresses");

            migrationBuilder.AlterColumn<int>(
                name: "ProgramId",
                table: "Redresses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Redresses_Programs_ProgramId",
                table: "Redresses",
                column: "ProgramId",
                principalTable: "Programs",
                principalColumn: "ProgramId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Redresses_Programs_ProgramId",
                table: "Redresses");

            migrationBuilder.AlterColumn<int>(
                name: "ProgramId",
                table: "Redresses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Redresses_Programs_ProgramId",
                table: "Redresses",
                column: "ProgramId",
                principalTable: "Programs",
                principalColumn: "ProgramId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
