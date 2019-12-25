using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class WaterSurfacePostData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WaterSurfacePostData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DateOfAnalysis = table.Column<DateTime>(type: "timestamp", nullable: false),
                    DateOfSampling = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    WaterContaminantId = table.Column<int>(type: "int4", nullable: false),
                    WaterSurfacePostId = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterSurfacePostData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterSurfacePostData_WaterContaminant_WaterContaminantId",
                        column: x => x.WaterContaminantId,
                        principalTable: "WaterContaminant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WaterSurfacePostData_WaterSurfacePost_WaterSurfacePostId",
                        column: x => x.WaterSurfacePostId,
                        principalTable: "WaterSurfacePost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaterSurfacePostData_WaterContaminantId",
                table: "WaterSurfacePostData",
                column: "WaterContaminantId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterSurfacePostData_WaterSurfacePostId",
                table: "WaterSurfacePostData",
                column: "WaterSurfacePostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaterSurfacePostData");
        }
    }
}
