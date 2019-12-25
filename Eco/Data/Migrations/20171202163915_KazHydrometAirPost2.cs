using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class KazHydrometAirPost2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SamplingTermId",
                table: "KazHydrometAirPost",
                type: "int4",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "SamplingType",
                table: "KazHydrometAirPost",
                type: "bool",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Type",
                table: "KazHydrometAirPost",
                type: "bool",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_KazHydrometAirPost_SamplingTermId",
                table: "KazHydrometAirPost",
                column: "SamplingTermId");

            migrationBuilder.AddForeignKey(
                name: "FK_KazHydrometAirPost_SamplingTerm_SamplingTermId",
                table: "KazHydrometAirPost",
                column: "SamplingTermId",
                principalTable: "SamplingTerm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KazHydrometAirPost_SamplingTerm_SamplingTermId",
                table: "KazHydrometAirPost");

            migrationBuilder.DropIndex(
                name: "IX_KazHydrometAirPost_SamplingTermId",
                table: "KazHydrometAirPost");

            migrationBuilder.DropColumn(
                name: "SamplingTermId",
                table: "KazHydrometAirPost");

            migrationBuilder.DropColumn(
                name: "SamplingType",
                table: "KazHydrometAirPost");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "KazHydrometAirPost");
        }
    }
}
