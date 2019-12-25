using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class AirPostData2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Benzapyrene",
                table: "AirPostData");

            migrationBuilder.DropColumn(
                name: "COmgm3",
                table: "AirPostData");

            migrationBuilder.DropColumn(
                name: "CarbonBlackmgm3",
                table: "AirPostData");

            migrationBuilder.DropColumn(
                name: "NO2mgm3",
                table: "AirPostData");

            migrationBuilder.DropColumn(
                name: "NOmg3",
                table: "AirPostData");

            migrationBuilder.DropColumn(
                name: "SO2mgm3",
                table: "AirPostData");

            migrationBuilder.DropColumn(
                name: "SuspendedSubstances",
                table: "AirPostData");

            migrationBuilder.AddColumn<int>(
                name: "AirContaminantId",
                table: "AirPostData",
                type: "int4",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "AirPostData",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_AirPostData_AirContaminantId",
                table: "AirPostData",
                column: "AirContaminantId");

            migrationBuilder.AddForeignKey(
                name: "FK_AirPostData_AirContaminant_AirContaminantId",
                table: "AirPostData",
                column: "AirContaminantId",
                principalTable: "AirContaminant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AirPostData_AirContaminant_AirContaminantId",
                table: "AirPostData");

            migrationBuilder.DropIndex(
                name: "IX_AirPostData_AirContaminantId",
                table: "AirPostData");

            migrationBuilder.DropColumn(
                name: "AirContaminantId",
                table: "AirPostData");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "AirPostData");

            migrationBuilder.AddColumn<decimal>(
                name: "Benzapyrene",
                table: "AirPostData",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "COmgm3",
                table: "AirPostData",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CarbonBlackmgm3",
                table: "AirPostData",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NO2mgm3",
                table: "AirPostData",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NOmg3",
                table: "AirPostData",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SO2mgm3",
                table: "AirPostData",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SuspendedSubstances",
                table: "AirPostData",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
