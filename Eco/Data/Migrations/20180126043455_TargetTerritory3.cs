using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class TargetTerritory3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AirPostId",
                table: "TargetTerritory",
                type: "int4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KazHydrometAirPostId",
                table: "TargetTerritory",
                type: "int4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KazHydrometSoilPostId",
                table: "TargetTerritory",
                type: "int4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KazHydrometWaterPostId",
                table: "TargetTerritory",
                type: "int4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoilPostId",
                table: "TargetTerritory",
                type: "int4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransportPostId",
                table: "TargetTerritory",
                type: "int4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WaterSurfacePostId",
                table: "TargetTerritory",
                type: "int4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TargetTerritory_AirPostId",
                table: "TargetTerritory",
                column: "AirPostId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetTerritory_KazHydrometAirPostId",
                table: "TargetTerritory",
                column: "KazHydrometAirPostId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetTerritory_KazHydrometSoilPostId",
                table: "TargetTerritory",
                column: "KazHydrometSoilPostId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetTerritory_KazHydrometWaterPostId",
                table: "TargetTerritory",
                column: "KazHydrometWaterPostId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetTerritory_SoilPostId",
                table: "TargetTerritory",
                column: "SoilPostId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetTerritory_TransportPostId",
                table: "TargetTerritory",
                column: "TransportPostId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetTerritory_WaterSurfacePostId",
                table: "TargetTerritory",
                column: "WaterSurfacePostId");

            migrationBuilder.AddForeignKey(
                name: "FK_TargetTerritory_AirPost_AirPostId",
                table: "TargetTerritory",
                column: "AirPostId",
                principalTable: "AirPost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TargetTerritory_KazHydrometAirPost_KazHydrometAirPostId",
                table: "TargetTerritory",
                column: "KazHydrometAirPostId",
                principalTable: "KazHydrometAirPost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TargetTerritory_KazHydrometSoilPost_KazHydrometSoilPostId",
                table: "TargetTerritory",
                column: "KazHydrometSoilPostId",
                principalTable: "KazHydrometSoilPost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TargetTerritory_KazHydrometWaterPost_KazHydrometWaterPostId",
                table: "TargetTerritory",
                column: "KazHydrometWaterPostId",
                principalTable: "KazHydrometWaterPost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TargetTerritory_SoilPost_SoilPostId",
                table: "TargetTerritory",
                column: "SoilPostId",
                principalTable: "SoilPost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TargetTerritory_TransportPost_TransportPostId",
                table: "TargetTerritory",
                column: "TransportPostId",
                principalTable: "TransportPost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TargetTerritory_WaterSurfacePost_WaterSurfacePostId",
                table: "TargetTerritory",
                column: "WaterSurfacePostId",
                principalTable: "WaterSurfacePost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TargetTerritory_AirPost_AirPostId",
                table: "TargetTerritory");

            migrationBuilder.DropForeignKey(
                name: "FK_TargetTerritory_KazHydrometAirPost_KazHydrometAirPostId",
                table: "TargetTerritory");

            migrationBuilder.DropForeignKey(
                name: "FK_TargetTerritory_KazHydrometSoilPost_KazHydrometSoilPostId",
                table: "TargetTerritory");

            migrationBuilder.DropForeignKey(
                name: "FK_TargetTerritory_KazHydrometWaterPost_KazHydrometWaterPostId",
                table: "TargetTerritory");

            migrationBuilder.DropForeignKey(
                name: "FK_TargetTerritory_SoilPost_SoilPostId",
                table: "TargetTerritory");

            migrationBuilder.DropForeignKey(
                name: "FK_TargetTerritory_TransportPost_TransportPostId",
                table: "TargetTerritory");

            migrationBuilder.DropForeignKey(
                name: "FK_TargetTerritory_WaterSurfacePost_WaterSurfacePostId",
                table: "TargetTerritory");

            migrationBuilder.DropIndex(
                name: "IX_TargetTerritory_AirPostId",
                table: "TargetTerritory");

            migrationBuilder.DropIndex(
                name: "IX_TargetTerritory_KazHydrometAirPostId",
                table: "TargetTerritory");

            migrationBuilder.DropIndex(
                name: "IX_TargetTerritory_KazHydrometSoilPostId",
                table: "TargetTerritory");

            migrationBuilder.DropIndex(
                name: "IX_TargetTerritory_KazHydrometWaterPostId",
                table: "TargetTerritory");

            migrationBuilder.DropIndex(
                name: "IX_TargetTerritory_SoilPostId",
                table: "TargetTerritory");

            migrationBuilder.DropIndex(
                name: "IX_TargetTerritory_TransportPostId",
                table: "TargetTerritory");

            migrationBuilder.DropIndex(
                name: "IX_TargetTerritory_WaterSurfacePostId",
                table: "TargetTerritory");

            migrationBuilder.DropColumn(
                name: "AirPostId",
                table: "TargetTerritory");

            migrationBuilder.DropColumn(
                name: "KazHydrometAirPostId",
                table: "TargetTerritory");

            migrationBuilder.DropColumn(
                name: "KazHydrometSoilPostId",
                table: "TargetTerritory");

            migrationBuilder.DropColumn(
                name: "KazHydrometWaterPostId",
                table: "TargetTerritory");

            migrationBuilder.DropColumn(
                name: "SoilPostId",
                table: "TargetTerritory");

            migrationBuilder.DropColumn(
                name: "TransportPostId",
                table: "TargetTerritory");

            migrationBuilder.DropColumn(
                name: "WaterSurfacePostId",
                table: "TargetTerritory");
        }
    }
}
