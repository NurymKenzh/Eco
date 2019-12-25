using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class KazHydrometWaterPostData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KazHydrometWaterPostData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    KazHydrometWaterPostId = table.Column<int>(type: "int4", nullable: false),
                    Month = table.Column<int>(type: "int4", nullable: false),
                    PollutantConcentrationmgl = table.Column<decimal>(type: "numeric", nullable: false),
                    WaterContaminantId = table.Column<int>(type: "int4", nullable: false),
                    Year = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KazHydrometWaterPostData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KazHydrometWaterPostData_KazHydrometWaterPost_KazHydrometWaterPostId",
                        column: x => x.KazHydrometWaterPostId,
                        principalTable: "KazHydrometWaterPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KazHydrometWaterPostData_WaterContaminant_WaterContaminantId",
                        column: x => x.WaterContaminantId,
                        principalTable: "WaterContaminant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KazHydrometWaterPostData_KazHydrometWaterPostId",
                table: "KazHydrometWaterPostData",
                column: "KazHydrometWaterPostId");

            migrationBuilder.CreateIndex(
                name: "IX_KazHydrometWaterPostData_WaterContaminantId",
                table: "KazHydrometWaterPostData",
                column: "WaterContaminantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KazHydrometWaterPostData");
        }
    }
}
