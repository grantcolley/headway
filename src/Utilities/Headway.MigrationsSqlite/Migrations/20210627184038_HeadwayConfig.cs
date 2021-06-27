using Microsoft.EntityFrameworkCore.Migrations;

namespace Headway.MigrationsSqlite.Migrations
{
    public partial class HeadwayConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModelConfigs",
                columns: table => new
                {
                    ModelConfigId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModelName = table.Column<string>(type: "TEXT", nullable: true),
                    ConfigApiPath = table.Column<string>(type: "TEXT", nullable: true),
                    RedirectText = table.Column<string>(type: "TEXT", nullable: true),
                    RedirectPage = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelConfigs", x => x.ModelConfigId);
                });

            migrationBuilder.CreateTable(
                name: "FieldConfigs",
                columns: table => new
                {
                    FieldConfigId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    IsIdField = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsTitleField = table.Column<bool>(type: "INTEGER", nullable: false),
                    PropertyName = table.Column<string>(type: "TEXT", nullable: true),
                    DynamicComponentTypeName = table.Column<string>(type: "TEXT", nullable: true),
                    ModelConfigId = table.Column<int>(type: "INTEGER", nullable: true)
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
                unique: true);
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
