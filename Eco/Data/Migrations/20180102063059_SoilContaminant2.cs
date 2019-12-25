using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class SoilContaminant2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoilContaminant_LimitingSoilIndicator_LimitingSoilIndicatorId",
                table: "SoilContaminant");

            migrationBuilder.DropIndex(
                name: "IX_SoilContaminant_LimitingSoilIndicatorId",
                table: "SoilContaminant");

            migrationBuilder.DropColumn(
                name: "LimitingSoilIndicatorId",
                table: "SoilContaminant");

            migrationBuilder.AddColumn<int>(
                name: "NormativeSoilTypeId",
                table: "SoilContaminant",
                type: "int4",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SoilContaminant_NormativeSoilTypeId",
                table: "SoilContaminant",
                column: "NormativeSoilTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SoilContaminant_NormativeSoilType_NormativeSoilTypeId",
                table: "SoilContaminant",
                column: "NormativeSoilTypeId",
                principalTable: "NormativeSoilType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoilContaminant_NormativeSoilType_NormativeSoilTypeId",
                table: "SoilContaminant");

            migrationBuilder.DropIndex(
                name: "IX_SoilContaminant_NormativeSoilTypeId",
                table: "SoilContaminant");

            migrationBuilder.DropColumn(
                name: "NormativeSoilTypeId",
                table: "SoilContaminant");

            migrationBuilder.AddColumn<int>(
                name: "LimitingSoilIndicatorId",
                table: "SoilContaminant",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SoilContaminant_LimitingSoilIndicatorId",
                table: "SoilContaminant",
                column: "LimitingSoilIndicatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_SoilContaminant_LimitingSoilIndicator_LimitingSoilIndicatorId",
                table: "SoilContaminant",
                column: "LimitingSoilIndicatorId",
                principalTable: "LimitingSoilIndicator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
