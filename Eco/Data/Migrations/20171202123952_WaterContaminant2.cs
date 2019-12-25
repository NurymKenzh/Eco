using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class WaterContaminant2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WaterContaminant_LimitingIndicator_LimitingIndicatorId",
                table: "WaterContaminant");

            migrationBuilder.DropForeignKey(
                name: "FK_WaterContaminant_SubstanceHazardClass_SubstanceHazardClassId",
                table: "WaterContaminant");

            migrationBuilder.DropIndex(
                name: "IX_WaterContaminant_LimitingIndicatorId",
                table: "WaterContaminant");

            migrationBuilder.DropIndex(
                name: "IX_WaterContaminant_SubstanceHazardClassId",
                table: "WaterContaminant");

            migrationBuilder.DropColumn(
                name: "LimitingIndicatorId",
                table: "WaterContaminant");

            migrationBuilder.DropColumn(
                name: "MaximumPermissibleConcentrationWater",
                table: "WaterContaminant");

            migrationBuilder.DropColumn(
                name: "NumberCAS",
                table: "WaterContaminant");

            migrationBuilder.DropColumn(
                name: "SubstanceHazardClassId",
                table: "WaterContaminant");

            migrationBuilder.AddColumn<decimal>(
                name: "Class1From",
                table: "WaterContaminant",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Class1To",
                table: "WaterContaminant",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Class2From",
                table: "WaterContaminant",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Class2To",
                table: "WaterContaminant",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Class3From",
                table: "WaterContaminant",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Class3To",
                table: "WaterContaminant",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Class4From",
                table: "WaterContaminant",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Class4To",
                table: "WaterContaminant",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Class5From",
                table: "WaterContaminant",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Class5To",
                table: "WaterContaminant",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "MeasurementUnitId",
                table: "WaterContaminant",
                type: "int4",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WaterContaminant_MeasurementUnitId",
                table: "WaterContaminant",
                column: "MeasurementUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_WaterContaminant_MeasurementUnit_MeasurementUnitId",
                table: "WaterContaminant",
                column: "MeasurementUnitId",
                principalTable: "MeasurementUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WaterContaminant_MeasurementUnit_MeasurementUnitId",
                table: "WaterContaminant");

            migrationBuilder.DropIndex(
                name: "IX_WaterContaminant_MeasurementUnitId",
                table: "WaterContaminant");

            migrationBuilder.DropColumn(
                name: "Class1From",
                table: "WaterContaminant");

            migrationBuilder.DropColumn(
                name: "Class1To",
                table: "WaterContaminant");

            migrationBuilder.DropColumn(
                name: "Class2From",
                table: "WaterContaminant");

            migrationBuilder.DropColumn(
                name: "Class2To",
                table: "WaterContaminant");

            migrationBuilder.DropColumn(
                name: "Class3From",
                table: "WaterContaminant");

            migrationBuilder.DropColumn(
                name: "Class3To",
                table: "WaterContaminant");

            migrationBuilder.DropColumn(
                name: "Class4From",
                table: "WaterContaminant");

            migrationBuilder.DropColumn(
                name: "Class4To",
                table: "WaterContaminant");

            migrationBuilder.DropColumn(
                name: "Class5From",
                table: "WaterContaminant");

            migrationBuilder.DropColumn(
                name: "Class5To",
                table: "WaterContaminant");

            migrationBuilder.DropColumn(
                name: "MeasurementUnitId",
                table: "WaterContaminant");

            migrationBuilder.AddColumn<int>(
                name: "LimitingIndicatorId",
                table: "WaterContaminant",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaximumPermissibleConcentrationWater",
                table: "WaterContaminant",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumberCAS",
                table: "WaterContaminant",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubstanceHazardClassId",
                table: "WaterContaminant",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WaterContaminant_LimitingIndicatorId",
                table: "WaterContaminant",
                column: "LimitingIndicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterContaminant_SubstanceHazardClassId",
                table: "WaterContaminant",
                column: "SubstanceHazardClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_WaterContaminant_LimitingIndicator_LimitingIndicatorId",
                table: "WaterContaminant",
                column: "LimitingIndicatorId",
                principalTable: "LimitingIndicator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WaterContaminant_SubstanceHazardClass_SubstanceHazardClassId",
                table: "WaterContaminant",
                column: "SubstanceHazardClassId",
                principalTable: "SubstanceHazardClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
