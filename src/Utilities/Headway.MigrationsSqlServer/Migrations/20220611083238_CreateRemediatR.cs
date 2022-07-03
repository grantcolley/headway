using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class CreateRemediatR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Roles",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Permissions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Permissions",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

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
                    Address1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address5 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PostCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SortCode = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
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
                    Compensation = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompensatoryInterest = table.Column<decimal>(type: "decimal(4,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.ProgramId);
                });

            migrationBuilder.CreateTable(
                name: "RefundCalculation",
                columns: table => new
                {
                    RefundCalculationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasicRefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompensatoryAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompensatoryInterestAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalCompensatoryAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalRefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CalculatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CalculatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundCalculation", x => x.RefundCalculationId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    RateType = table.Column<int>(type: "int", nullable: false),
                    RepaymentType = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId");
                });

            migrationBuilder.CreateTable(
                name: "Redresses",
                columns: table => new
                {
                    RedressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    ProgramId = table.Column<int>(type: "int", nullable: true),
                    RefundCalculationId = table.Column<int>(type: "int", nullable: true),
                    RefundVerificationRefundCalculationId = table.Column<int>(type: "int", nullable: true),
                    RedressCaseOwner = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RedressLoadBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RedressLoadDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    ResponseRecieved = table.Column<bool>(type: "bit", nullable: true),
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
                    FinalRedressReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Redresses", x => x.RedressId);
                    table.ForeignKey(
                        name: "FK_Redresses_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_Redresses_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "ProgramId");
                    table.ForeignKey(
                        name: "FK_Redresses_RefundCalculation_RefundCalculationId",
                        column: x => x.RefundCalculationId,
                        principalTable: "RefundCalculation",
                        principalColumn: "RefundCalculationId");
                    table.ForeignKey(
                        name: "FK_Redresses_RefundCalculation_RefundVerificationRefundCalculationId",
                        column: x => x.RefundVerificationRefundCalculationId,
                        principalTable: "RefundCalculation",
                        principalColumn: "RefundCalculationId");
                });

            migrationBuilder.CreateTable(
                name: "RedressProducts",
                columns: table => new
                {
                    RedressProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RedressId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedressProducts", x => x.RedressProductId);
                    table.ForeignKey(
                        name: "FK_RedressProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RedressProducts_Redresses_RedressId",
                        column: x => x.RedressId,
                        principalTable: "Redresses",
                        principalColumn: "RedressId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CustomerId",
                table: "Products",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Redresses_CustomerId",
                table: "Redresses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Redresses_ProgramId",
                table: "Redresses",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Redresses_RefundCalculationId",
                table: "Redresses",
                column: "RefundCalculationId");

            migrationBuilder.CreateIndex(
                name: "IX_Redresses_RefundVerificationRefundCalculationId",
                table: "Redresses",
                column: "RefundVerificationRefundCalculationId");

            migrationBuilder.CreateIndex(
                name: "IX_RedressProducts_ProductId",
                table: "RedressProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RedressProducts_RedressId",
                table: "RedressProducts",
                column: "RedressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RedressProducts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Redresses");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropTable(
                name: "RefundCalculation");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roles",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Roles",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Permissions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Permissions",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);
        }
    }
}
