using Microsoft.EntityFrameworkCore.Migrations;

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class HeadwayConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModelConfigs",
                columns: table => new
                {
                    ModelConfigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ConfigApiPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RedirectText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RedirectPage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelConfigs", x => x.ModelConfigId);
                });

            migrationBuilder.CreateTable(
                name: "FieldConfigs",
                columns: table => new
                {
                    FieldConfigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsIdField = table.Column<bool>(type: "bit", nullable: false),
                    IsTitleField = table.Column<bool>(type: "bit", nullable: false),
                    PropertyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DynamicComponentTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModelConfigId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldConfigs", x => x.FieldConfigId);
                    table.ForeignKey(
                        name: "FK_FieldConfigs_ModelConfigs_ModelConfigId",
                        column: x => x.ModelConfigId,
                        principalTable: "ModelConfigs",
                        principalColumn: "ModelConfigId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FieldConfigs_ModelConfigId",
                table: "FieldConfigs",
                column: "ModelConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelConfigs_ModelName",
                table: "ModelConfigs",
                column: "ModelName",
                unique: true,
                filter: "[ModelName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldConfigs");

            migrationBuilder.DropTable(
                name: "ModelConfigs");
        }
    }
}
