using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class CompanyEmissionsValue2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoefficientOfSettlement",
                table: "CompanyEmissionsValue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CoefficientOfSettlement",
                table: "CompanyEmissionsValue",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
