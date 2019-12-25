using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class KazHydrometAirPostData2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PollutantConcentrationMonthlyAverage",
                table: "KazHydrometAirPostData",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "PollutantConcentrationMaximumOneTimePerMonth",
                table: "KazHydrometAirPostData",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "Month",
                table: "KazHydrometAirPostData",
                type: "int4",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "PollutantConcentrationMaximumOneTimeMonth",
                table: "KazHydrometAirPostData",
                type: "int4",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PollutantConcentrationMaximumOneTimePerYear",
                table: "KazHydrometAirPostData",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PollutantConcentrationYearlyAverage",
                table: "KazHydrometAirPostData",
                type: "numeric",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PollutantConcentrationMaximumOneTimeMonth",
                table: "KazHydrometAirPostData");

            migrationBuilder.DropColumn(
                name: "PollutantConcentrationMaximumOneTimePerYear",
                table: "KazHydrometAirPostData");

            migrationBuilder.DropColumn(
                name: "PollutantConcentrationYearlyAverage",
                table: "KazHydrometAirPostData");

            migrationBuilder.AlterColumn<decimal>(
                name: "PollutantConcentrationMonthlyAverage",
                table: "KazHydrometAirPostData",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PollutantConcentrationMaximumOneTimePerMonth",
                table: "KazHydrometAirPostData",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Month",
                table: "KazHydrometAirPostData",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4",
                oldNullable: true);
        }
    }
}
