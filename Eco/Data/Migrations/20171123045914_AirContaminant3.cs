using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class AirContaminant3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ApproximateSafeExposureLevel",
                table: "AirContaminant",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LimitingIndicatorId",
                table: "AirContaminant",
                type: "int4",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaximumPermissibleConcentrationDailyAverage",
                table: "AirContaminant",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaximumPermissibleConcentrationOneTimemaximum",
                table: "AirContaminant",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PresenceOfTheMaximumPermissibleConcentration",
                table: "AirContaminant",
                type: "bool",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SubstanceHazardClassId",
                table: "AirContaminant",
                type: "int4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AirContaminant_LimitingIndicatorId",
                table: "AirContaminant",
                column: "LimitingIndicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AirContaminant_SubstanceHazardClassId",
                table: "AirContaminant",
                column: "SubstanceHazardClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_AirContaminant_LimitingIndicator_LimitingIndicatorId",
                table: "AirContaminant",
                column: "LimitingIndicatorId",
                principalTable: "LimitingIndicator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AirContaminant_SubstanceHazardClass_SubstanceHazardClassId",
                table: "AirContaminant",
                column: "SubstanceHazardClassId",
                principalTable: "SubstanceHazardClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AirContaminant_LimitingIndicator_LimitingIndicatorId",
                table: "AirContaminant");

            migrationBuilder.DropForeignKey(
                name: "FK_AirContaminant_SubstanceHazardClass_SubstanceHazardClassId",
                table: "AirContaminant");

            migrationBuilder.DropIndex(
                name: "IX_AirContaminant_LimitingIndicatorId",
                table: "AirContaminant");

            migrationBuilder.DropIndex(
                name: "IX_AirContaminant_SubstanceHazardClassId",
                table: "AirContaminant");

            migrationBuilder.DropColumn(
                name: "ApproximateSafeExposureLevel",
                table: "AirContaminant");

            migrationBuilder.DropColumn(
                name: "LimitingIndicatorId",
                table: "AirContaminant");

            migrationBuilder.DropColumn(
                name: "MaximumPermissibleConcentrationDailyAverage",
                table: "AirContaminant");

            migrationBuilder.DropColumn(
                name: "MaximumPermissibleConcentrationOneTimemaximum",
                table: "AirContaminant");

            migrationBuilder.DropColumn(
                name: "PresenceOfTheMaximumPermissibleConcentration",
                table: "AirContaminant");

            migrationBuilder.DropColumn(
                name: "SubstanceHazardClassId",
                table: "AirContaminant");
        }
    }
}
