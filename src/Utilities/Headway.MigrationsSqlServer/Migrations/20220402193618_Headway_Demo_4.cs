using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class Headway_Demo_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DropdownComplexId",
                table: "DemoModels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DemoModelComplexProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoModelComplexProperties", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemoModels_DropdownComplexId",
                table: "DemoModels",
                column: "DropdownComplexId");

            migrationBuilder.AddForeignKey(
                name: "FK_DemoModels_DemoModelComplexProperties_DropdownComplexId",
                table: "DemoModels",
                column: "DropdownComplexId",
                principalTable: "DemoModelComplexProperties",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemoModels_DemoModelComplexProperties_DropdownComplexId",
                table: "DemoModels");

            migrationBuilder.DropTable(
                name: "DemoModelComplexProperties");

            migrationBuilder.DropIndex(
                name: "IX_DemoModels_DropdownComplexId",
                table: "DemoModels");

            migrationBuilder.DropColumn(
                name: "DropdownComplexId",
                table: "DemoModels");
        }
    }
}
