using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class Headway_Redress_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Redresses_RefundCalculation_RefundCalculationId",
                table: "Redresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Redresses_RefundCalculation_RefundVerificationRefundCalculationId",
                table: "Redresses");

            migrationBuilder.DropIndex(
                name: "IX_Redresses_RefundVerificationRefundCalculationId",
                table: "Redresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefundCalculation",
                table: "RefundCalculation");

            migrationBuilder.DropColumn(
                name: "RefundVerificationRefundCalculationId",
                table: "Redresses");

            migrationBuilder.RenameTable(
                name: "RefundCalculation",
                newName: "RefundCalculations");

            migrationBuilder.AlterColumn<int>(
                name: "RefundCalculationId",
                table: "Redresses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VerifiedBasicRefundAmount",
                table: "RefundCalculations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerifiedBy",
                table: "RefundCalculations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VerifiedCompensatoryAmount",
                table: "RefundCalculations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VerifiedCompensatoryInterestAmount",
                table: "RefundCalculations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedDate",
                table: "RefundCalculations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VerifiedTotalCompensatoryAmount",
                table: "RefundCalculations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VerifiedTotalRefundAmount",
                table: "RefundCalculations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefundCalculations",
                table: "RefundCalculations",
                column: "RefundCalculationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Redresses_RefundCalculations_RefundCalculationId",
                table: "Redresses",
                column: "RefundCalculationId",
                principalTable: "RefundCalculations",
                principalColumn: "RefundCalculationId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Redresses_RefundCalculations_RefundCalculationId",
                table: "Redresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefundCalculations",
                table: "RefundCalculations");

            migrationBuilder.DropColumn(
                name: "VerifiedBasicRefundAmount",
                table: "RefundCalculations");

            migrationBuilder.DropColumn(
                name: "VerifiedBy",
                table: "RefundCalculations");

            migrationBuilder.DropColumn(
                name: "VerifiedCompensatoryAmount",
                table: "RefundCalculations");

            migrationBuilder.DropColumn(
                name: "VerifiedCompensatoryInterestAmount",
                table: "RefundCalculations");

            migrationBuilder.DropColumn(
                name: "VerifiedDate",
                table: "RefundCalculations");

            migrationBuilder.DropColumn(
                name: "VerifiedTotalCompensatoryAmount",
                table: "RefundCalculations");

            migrationBuilder.DropColumn(
                name: "VerifiedTotalRefundAmount",
                table: "RefundCalculations");

            migrationBuilder.RenameTable(
                name: "RefundCalculations",
                newName: "RefundCalculation");

            migrationBuilder.AlterColumn<int>(
                name: "RefundCalculationId",
                table: "Redresses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RefundVerificationRefundCalculationId",
                table: "Redresses",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefundCalculation",
                table: "RefundCalculation",
                column: "RefundCalculationId");

            migrationBuilder.CreateIndex(
                name: "IX_Redresses_RefundVerificationRefundCalculationId",
                table: "Redresses",
                column: "RefundVerificationRefundCalculationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Redresses_RefundCalculation_RefundCalculationId",
                table: "Redresses",
                column: "RefundCalculationId",
                principalTable: "RefundCalculation",
                principalColumn: "RefundCalculationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Redresses_RefundCalculation_RefundVerificationRefundCalculationId",
                table: "Redresses",
                column: "RefundVerificationRefundCalculationId",
                principalTable: "RefundCalculation",
                principalColumn: "RefundCalculationId");
        }
    }
}
