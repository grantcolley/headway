using Microsoft.EntityFrameworkCore.Migrations;

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class HeadwayConfigUpdate4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldConfigs");

            migrationBuilder.DropTable(
                name: "ListItemConfigs");

            migrationBuilder.DropTable(
                name: "ModelConfigs");

            migrationBuilder.DropTable(
                name: "ListConfigs");

            migrationBuilder.AddColumn<string>(
                name: "Config",
                table: "ConfigTypes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Config",
                table: "ConfigTypes");

            migrationBuilder.CreateTable(
                name: "ListConfigs",
                columns: table => new
                {
                    ListConfigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Component = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfigApi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NavigateTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NavigateToConfig = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NavigationPropertyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListConfigs", x => x.ListConfigId);
                });

            migrationBuilder.CreateTable(
                name: "ModelConfigs",
                columns: table => new
                {
                    ModelConfigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfigApi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NavigateText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NavigateTo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelConfigs", x => x.ModelConfigId);
                });

            migrationBuilder.CreateTable(
                name: "ListItemConfigs",
                columns: table => new
                {
                    ListItemConfigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ListConfigId = table.Column<int>(type: "int", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    PropertyName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListItemConfigs", x => x.ListItemConfigId);
                    table.ForeignKey(
                        name: "FK_ListItemConfigs_ListConfigs_ListConfigId",
                        column: x => x.ListConfigId,
                        principalTable: "ListConfigs",
                        principalColumn: "ListConfigId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FieldConfigs",
                columns: table => new
                {
                    FieldConfigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DynamicComponentTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsIdField = table.Column<bool>(type: "bit", nullable: false),
                    IsTitleField = table.Column<bool>(type: "bit", nullable: false),
                    ModelConfigId = table.Column<int>(type: "int", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    PropertyName = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "IX_ListConfigs_Name",
                table: "ListConfigs",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ListItemConfigs_ListConfigId",
                table: "ListItemConfigs",
                column: "ListConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelConfigs_Model",
                table: "ModelConfigs",
                column: "Model",
                unique: true,
                filter: "[Model] IS NOT NULL");
        }
    }
}
