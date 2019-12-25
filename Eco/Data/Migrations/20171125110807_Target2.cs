using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class Target2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MeasurementUnitId",
                table: "Target",
                type: "int4",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Target_MeasurementUnitId",
                table: "Target",
                column: "MeasurementUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Target_MeasurementUnit_MeasurementUnitId",
                table: "Target",
                column: "MeasurementUnitId",
                principalTable: "MeasurementUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Target_MeasurementUnit_MeasurementUnitId",
                table: "Target");

            migrationBuilder.DropIndex(
                name: "IX_Target_MeasurementUnitId",
                table: "Target");

            migrationBuilder.DropColumn(
                name: "MeasurementUnitId",
                table: "Target");
        }
    }
}
