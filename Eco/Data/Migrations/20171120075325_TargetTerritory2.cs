using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class TargetTerritory2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TargetTerritory_CityDistrict_CityDistrictId",
                table: "TargetTerritory");

            migrationBuilder.AlterColumn<int>(
                name: "CityDistrictId",
                table: "TargetTerritory",
                type: "int4",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_TargetTerritory_CityDistrict_CityDistrictId",
                table: "TargetTerritory",
                column: "CityDistrictId",
                principalTable: "CityDistrict",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TargetTerritory_CityDistrict_CityDistrictId",
                table: "TargetTerritory");

            migrationBuilder.AlterColumn<int>(
                name: "CityDistrictId",
                table: "TargetTerritory",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TargetTerritory_CityDistrict_CityDistrictId",
                table: "TargetTerritory",
                column: "CityDistrictId",
                principalTable: "CityDistrict",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
