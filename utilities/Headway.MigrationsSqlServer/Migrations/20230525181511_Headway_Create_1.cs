using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class HeadwayCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Audits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClrType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountStatus = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SortCode = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Address1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address5 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "DemoModelComplexProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoModelComplexProperties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Flows",
                columns: table => new
                {
                    FlowId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlowStatus = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FlowCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FlowConfigurationClass = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Permission = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flows", x => x.FlowId);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    ModuleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Permission = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.ModuleId);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionId);
                });

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    ProgramId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    RateType = table.Column<int>(type: "int", nullable: false),
                    RepaymentType = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Compensation = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompensatoryInterest = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.ProgramId);
                });

            migrationBuilder.CreateTable(
                name: "RefundCalculations",
                columns: table => new
                {
                    RefundCalculationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalculatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CalculatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BasicRefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompensatoryAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompensatoryInterestAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalCompensatoryAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalRefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VerifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VerifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedBasicRefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VerifiedCompensatoryAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VerifiedCompensatoryInterestAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VerifiedTotalCompensatoryAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VerifiedTotalRefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundCalculations", x => x.RefundCalculationId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    RateType = table.Column<int>(type: "int", nullable: false),
                    RepaymentType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DemoModels",
                columns: table => new
                {
                    DemoModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Checkbox = table.Column<bool>(type: "bit", nullable: false),
                    Integer = table.Column<int>(type: "int", nullable: false),
                    OptionVertical = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptionHorizontal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dropdown = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DropdownComplexId = table.Column<int>(type: "int", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TextMultiline = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Decimal = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoModels", x => x.DemoModelId);
                    table.ForeignKey(
                        name: "FK_DemoModels_DemoModelComplexProperties_DropdownComplexId",
                        column: x => x.DropdownComplexId,
                        principalTable: "DemoModelComplexProperties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    ConfigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlowId = table.Column<int>(type: "int", nullable: true),
                    NavigateResetBreadcrumb = table.Column<bool>(type: "bit", nullable: false),
                    CreateLocal = table.Column<bool>(type: "bit", nullable: false),
                    UseSearchComponent = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ModelApi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderModelBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Document = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DocumentArgs = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    NavigatePage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NavigateProperty = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NavigateConfig = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SearchComponent = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.ConfigId);
                    table.ForeignKey(
                        name: "FK_Configs_Flows_FlowId",
                        column: x => x.FlowId,
                        principalTable: "Flows",
                        principalColumn: "FlowId");
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    StateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Position = table.Column<int>(type: "int", nullable: false),
                    StateType = table.Column<int>(type: "int", nullable: false),
                    StateStatus = table.Column<int>(type: "int", nullable: false),
                    IsOwnerRestricted = table.Column<bool>(type: "bit", nullable: false),
                    FlowId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StateCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParentStateCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WritePermission = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReadPermission = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubStateCodes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TransitionStateCodes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    RegressionStateCodes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    StateConfigurationClass = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.StateId);
                    table.ForeignKey(
                        name: "FK_States_Flows_FlowId",
                        column: x => x.FlowId,
                        principalTable: "Flows",
                        principalColumn: "FlowId");
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Permission = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_Categories_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionRole",
                columns: table => new
                {
                    PermissionsPermissionId = table.Column<int>(type: "int", nullable: false),
                    RolesRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRole", x => new { x.PermissionsPermissionId, x.RolesRoleId });
                    table.ForeignKey(
                        name: "FK_PermissionRole_Permissions_PermissionsPermissionId",
                        column: x => x.PermissionsPermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionRole_Roles_RolesRoleId",
                        column: x => x.RolesRoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionUser",
                columns: table => new
                {
                    PermissionsPermissionId = table.Column<int>(type: "int", nullable: false),
                    UsersUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionUser", x => new { x.PermissionsPermissionId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_PermissionUser_Permissions_PermissionsPermissionId",
                        column: x => x.PermissionsPermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionUser_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RolesRoleId = table.Column<int>(type: "int", nullable: false),
                    UsersUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesRoleId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Roles_RolesRoleId",
                        column: x => x.RolesRoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Redresses",
                columns: table => new
                {
                    RedressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    RefundCalculationId = table.Column<int>(type: "int", nullable: false),
                    RefressFlowContextId = table.Column<int>(type: "int", nullable: false),
                    RedressCaseOwner = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RedressCreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RedressCreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefundAssessmentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RefundAssessmentBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RefundAssessmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefundReviewStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RefundReviewComment = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    RefundReviewBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RefundReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RedressReviewStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RedressReviewComment = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    RedressReviewBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RedressReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RedressValidationStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RedressValidationComment = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    RedressValidationBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RedressValidationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CommunicationGenerationStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CommunicationGenerationBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CommunicationGenerationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CommunicationDispatchStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CommunicationDispatchComment = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CommunicationDispatchBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CommunicationDispatchDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResponseRequired = table.Column<bool>(type: "bit", nullable: true),
                    ResponseReceived = table.Column<bool>(type: "bit", nullable: true),
                    AwaitingResponseStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AwaitingResponseComment = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    AwaitingResponseBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AwaitingResponseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentGenerationStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentGenerationBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentGenerationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinalRedressReviewStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FinalRedressReviewComment = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    FinalRedressReviewBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FinalRedressReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Redresses", x => x.RedressId);
                    table.ForeignKey(
                        name: "FK_Redresses_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Redresses_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Redresses_RefundCalculations_RefundCalculationId",
                        column: x => x.RefundCalculationId,
                        principalTable: "RefundCalculations",
                        principalColumn: "RefundCalculationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DemoModelItems",
                columns: table => new
                {
                    DemoModelItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemoModelId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoModelItems", x => x.DemoModelItemId);
                    table.ForeignKey(
                        name: "FK_DemoModelItems_DemoModels_DemoModelId",
                        column: x => x.DemoModelId,
                        principalTable: "DemoModels",
                        principalColumn: "DemoModelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DemoModelTreeItems",
                columns: table => new
                {
                    DemoModelTreeItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    DemoModelId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParentCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DemoModelTreeItemId1 = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoModelTreeItems", x => x.DemoModelTreeItemId);
                    table.ForeignKey(
                        name: "FK_DemoModelTreeItems_DemoModelTreeItems_DemoModelTreeItemId1",
                        column: x => x.DemoModelTreeItemId1,
                        principalTable: "DemoModelTreeItems",
                        principalColumn: "DemoModelTreeItemId");
                    table.ForeignKey(
                        name: "FK_DemoModelTreeItems_DemoModels_DemoModelId",
                        column: x => x.DemoModelId,
                        principalTable: "DemoModels",
                        principalColumn: "DemoModelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConfigContainers",
                columns: table => new
                {
                    ConfigContainerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfigId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Container = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParentCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ComponentArgs = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    FlowArgs = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    ConfigContainerId1 = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigContainers", x => x.ConfigContainerId);
                    table.ForeignKey(
                        name: "FK_ConfigContainers_ConfigContainers_ConfigContainerId1",
                        column: x => x.ConfigContainerId1,
                        principalTable: "ConfigContainers",
                        principalColumn: "ConfigContainerId");
                    table.ForeignKey(
                        name: "FK_ConfigContainers_Configs_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "Configs",
                        principalColumn: "ConfigId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConfigSearchItems",
                columns: table => new
                {
                    ConfigSearchItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParameterName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tooltip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Component = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ComponentArgs = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    ConfigId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigSearchItems", x => x.ConfigSearchItemId);
                    table.ForeignKey(
                        name: "FK_ConfigSearchItems_Configs_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "Configs",
                        principalColumn: "ConfigId");
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    MenuItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Permission = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NavigatePage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Config = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.MenuItemId);
                    table.ForeignKey(
                        name: "FK_MenuItems_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RedressFlowContexts",
                columns: table => new
                {
                    RedressFlowContextId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlowId = table.Column<int>(type: "int", nullable: false),
                    RedressId = table.Column<int>(type: "int", nullable: false),
                    CurrentUserUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedressFlowContexts", x => x.RedressFlowContextId);
                    table.ForeignKey(
                        name: "FK_RedressFlowContexts_Flows_FlowId",
                        column: x => x.FlowId,
                        principalTable: "Flows",
                        principalColumn: "FlowId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RedressFlowContexts_Redresses_RedressId",
                        column: x => x.RedressId,
                        principalTable: "Redresses",
                        principalColumn: "RedressId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RedressFlowContexts_Users_CurrentUserUserId",
                        column: x => x.CurrentUserUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConfigItems",
                columns: table => new
                {
                    ConfigItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfigId = table.Column<int>(type: "int", nullable: false),
                    IsIdentity = table.Column<bool>(type: "bit", nullable: false),
                    IsTitle = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ConfigContainerId = table.Column<int>(type: "int", nullable: true),
                    PropertyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tooltip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Component = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ComponentArgs = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    ConfigName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigItems", x => x.ConfigItemId);
                    table.ForeignKey(
                        name: "FK_ConfigItems_ConfigContainers_ConfigContainerId",
                        column: x => x.ConfigContainerId,
                        principalTable: "ConfigContainers",
                        principalColumn: "ConfigContainerId");
                    table.ForeignKey(
                        name: "FK_ConfigItems_Configs_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "Configs",
                        principalColumn: "ConfigId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RedressFlowHistory",
                columns: table => new
                {
                    RedressFlowHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RedressFlowContextId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateStatus = table.Column<int>(type: "int", nullable: false),
                    FlowCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StateCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Event = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedressFlowHistory", x => x.RedressFlowHistoryId);
                    table.ForeignKey(
                        name: "FK_RedressFlowHistory_RedressFlowContexts_RedressFlowContextId",
                        column: x => x.RedressFlowContextId,
                        principalTable: "RedressFlowContexts",
                        principalColumn: "RedressFlowContextId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ModuleId",
                table: "Categories",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfigContainers_ConfigContainerId1",
                table: "ConfigContainers",
                column: "ConfigContainerId1");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigContainers_ConfigId",
                table: "ConfigContainers",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigContainers_Name",
                table: "ConfigContainers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItems_ConfigContainerId",
                table: "ConfigItems",
                column: "ConfigContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItems_ConfigId",
                table: "ConfigItems",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Configs_FlowId",
                table: "Configs",
                column: "FlowId");

            migrationBuilder.CreateIndex(
                name: "IX_Configs_Name",
                table: "Configs",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfigSearchItems_ConfigId",
                table: "ConfigSearchItems",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                table: "Countries",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DemoModelItems_DemoModelId",
                table: "DemoModelItems",
                column: "DemoModelId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoModels_DropdownComplexId",
                table: "DemoModels",
                column: "DropdownComplexId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoModelTreeItems_DemoModelId",
                table: "DemoModelTreeItems",
                column: "DemoModelId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoModelTreeItems_DemoModelTreeItemId1",
                table: "DemoModelTreeItems",
                column: "DemoModelTreeItemId1");

            migrationBuilder.CreateIndex(
                name: "IX_Flows_Name",
                table: "Flows",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_CategoryId",
                table: "MenuItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_Name",
                table: "MenuItems",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_Name",
                table: "Modules",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRole_RolesRoleId",
                table: "PermissionRole",
                column: "RolesRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Name",
                table: "Permissions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionUser_UsersUserId",
                table: "PermissionUser",
                column: "UsersUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CustomerId",
                table: "Products",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_Name",
                table: "Programs",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Redresses_ProductId",
                table: "Redresses",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Redresses_ProgramId",
                table: "Redresses",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Redresses_RefundCalculationId",
                table: "Redresses",
                column: "RefundCalculationId");

            migrationBuilder.CreateIndex(
                name: "IX_RedressFlowContexts_CurrentUserUserId",
                table: "RedressFlowContexts",
                column: "CurrentUserUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RedressFlowContexts_FlowId",
                table: "RedressFlowContexts",
                column: "FlowId");

            migrationBuilder.CreateIndex(
                name: "IX_RedressFlowContexts_RedressId",
                table: "RedressFlowContexts",
                column: "RedressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RedressFlowHistory_RedressFlowContextId",
                table: "RedressFlowHistory",
                column: "RedressFlowContextId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UsersUserId",
                table: "RoleUser",
                column: "UsersUserId");

            migrationBuilder.CreateIndex(
                name: "IX_States_FlowId",
                table: "States",
                column: "FlowId");

            migrationBuilder.CreateIndex(
                name: "IX_States_StateCode",
                table: "States",
                column: "StateCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Audits");

            migrationBuilder.DropTable(
                name: "ConfigItems");

            migrationBuilder.DropTable(
                name: "ConfigSearchItems");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "DemoModelItems");

            migrationBuilder.DropTable(
                name: "DemoModelTreeItems");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "PermissionRole");

            migrationBuilder.DropTable(
                name: "PermissionUser");

            migrationBuilder.DropTable(
                name: "RedressFlowHistory");

            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "ConfigContainers");

            migrationBuilder.DropTable(
                name: "DemoModels");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "RedressFlowContexts");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "DemoModelComplexProperties");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Redresses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Flows");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropTable(
                name: "RefundCalculations");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
