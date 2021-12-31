using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class Headway_DemoModel_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DemoModels",
                columns: table => new
                {
                    DemoModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Style = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DemoModelId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoModels", x => x.DemoModelId);
                    table.ForeignKey(
                        name: "FK_DemoModels_DemoModels_DemoModelId1",
                        column: x => x.DemoModelId1,
                        principalTable: "DemoModels",
                        principalColumn: "DemoModelId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemoModels_DemoModelId1",
                table: "DemoModels",
                column: "DemoModelId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemoModels");
        }
    }
}
