using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class AirContaminant4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AirContaminant_SubstanceHazardClass_SubstanceHazardClassId",
                table: "AirContaminant");

            migrationBuilder.AlterColumn<int>(
                name: "SubstanceHazardClassId",
                table: "AirContaminant",
                type: "int4",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AirContaminant_SubstanceHazardClass_SubstanceHazardClassId",
                table: "AirContaminant",
                column: "SubstanceHazardClassId",
                principalTable: "SubstanceHazardClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AirContaminant_SubstanceHazardClass_SubstanceHazardClassId",
                table: "AirContaminant");

            migrationBuilder.AlterColumn<int>(
                name: "SubstanceHazardClassId",
                table: "AirContaminant",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.AddForeignKey(
                name: "FK_AirContaminant_SubstanceHazardClass_SubstanceHazardClassId",
                table: "AirContaminant",
                column: "SubstanceHazardClassId",
                principalTable: "SubstanceHazardClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
