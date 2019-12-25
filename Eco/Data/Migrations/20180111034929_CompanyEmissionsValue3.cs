using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class CompanyEmissionsValue3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "YearOfAchievementMaximumPermissibleEmissions",
                table: "CompanyEmissionsValue",
                type: "int4",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "ValuesMaximumPermissibleEmissionstyear",
                table: "CompanyEmissionsValue",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ValuesMaximumPermissibleEmissionsmgm3",
                table: "CompanyEmissionsValue",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ValuesMaximumPermissibleEmissionsgs",
                table: "CompanyEmissionsValue",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "YearOfAchievementMaximumPermissibleEmissions",
                table: "CompanyEmissionsValue",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValuesMaximumPermissibleEmissionstyear",
                table: "CompanyEmissionsValue",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValuesMaximumPermissibleEmissionsmgm3",
                table: "CompanyEmissionsValue",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValuesMaximumPermissibleEmissionsgs",
                table: "CompanyEmissionsValue",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);
        }
    }
}
