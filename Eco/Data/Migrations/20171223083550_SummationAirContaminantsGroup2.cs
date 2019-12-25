using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class SummationAirContaminantsGroup2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ApproximateSafeExposureLevel",
                table: "SummationAirContaminantsGroup",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LimitingIndicatorId",
                table: "SummationAirContaminantsGroup",
                type: "int4",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaximumPermissibleConcentrationDailyAverage",
                table: "SummationAirContaminantsGroup",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaximumPermissibleConcentrationOneTimemaximum",
                table: "SummationAirContaminantsGroup",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PresenceOfTheMaximumPermissibleConcentration",
                table: "SummationAirContaminantsGroup",
                type: "bool",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SubstanceHazardClassId",
                table: "SummationAirContaminantsGroup",
                type: "int4",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Number168",
                table: "AirContaminant",
                type: "int4",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Number104",
                table: "AirContaminant",
                type: "int4",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SummationAirContaminantsGroup_LimitingIndicatorId",
                table: "SummationAirContaminantsGroup",
                column: "LimitingIndicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_SummationAirContaminantsGroup_SubstanceHazardClassId",
                table: "SummationAirContaminantsGroup",
                column: "SubstanceHazardClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_SummationAirContaminantsGroup_LimitingIndicator_LimitingIndicatorId",
                table: "SummationAirContaminantsGroup",
                column: "LimitingIndicatorId",
                principalTable: "LimitingIndicator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SummationAirContaminantsGroup_SubstanceHazardClass_SubstanceHazardClassId",
                table: "SummationAirContaminantsGroup",
                column: "SubstanceHazardClassId",
                principalTable: "SubstanceHazardClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SummationAirContaminantsGroup_LimitingIndicator_LimitingIndicatorId",
                table: "SummationAirContaminantsGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_SummationAirContaminantsGroup_SubstanceHazardClass_SubstanceHazardClassId",
                table: "SummationAirContaminantsGroup");

            migrationBuilder.DropIndex(
                name: "IX_SummationAirContaminantsGroup_LimitingIndicatorId",
                table: "SummationAirContaminantsGroup");

            migrationBuilder.DropIndex(
                name: "IX_SummationAirContaminantsGroup_SubstanceHazardClassId",
                table: "SummationAirContaminantsGroup");

            migrationBuilder.DropColumn(
                name: "ApproximateSafeExposureLevel",
                table: "SummationAirContaminantsGroup");

            migrationBuilder.DropColumn(
                name: "LimitingIndicatorId",
                table: "SummationAirContaminantsGroup");

            migrationBuilder.DropColumn(
                name: "MaximumPermissibleConcentrationDailyAverage",
                table: "SummationAirContaminantsGroup");

            migrationBuilder.DropColumn(
                name: "MaximumPermissibleConcentrationOneTimemaximum",
                table: "SummationAirContaminantsGroup");

            migrationBuilder.DropColumn(
                name: "PresenceOfTheMaximumPermissibleConcentration",
                table: "SummationAirContaminantsGroup");

            migrationBuilder.DropColumn(
                name: "SubstanceHazardClassId",
                table: "SummationAirContaminantsGroup");

            migrationBuilder.AlterColumn<int>(
                name: "Number168",
                table: "AirContaminant",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.AlterColumn<int>(
                name: "Number104",
                table: "AirContaminant",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int4");
        }
    }
}
