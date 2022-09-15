using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Headway.MigrationsSqlServer.Migrations
{
    public partial class Headway_Redress_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefundAssessmentBy",
                table: "Redresses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefundAssessmentDate",
                table: "Redresses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefundAssessmentStatus",
                table: "Redresses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefundAssessmentBy",
                table: "Redresses");

            migrationBuilder.DropColumn(
                name: "RefundAssessmentDate",
                table: "Redresses");

            migrationBuilder.DropColumn(
                name: "RefundAssessmentStatus",
                table: "Redresses");
        }
    }
}
