using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class SoilPostData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SoilPostData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ConcentrationValuemgkg = table.Column<decimal>(type: "numeric", nullable: false),
                    DateOfSampling = table.Column<DateTime>(type: "timestamp", nullable: false),
                    GammaBackgroundOfTheSoil = table.Column<decimal>(type: "numeric", nullable: false),
                    SoilContaminantId = table.Column<int>(type: "int4", nullable: false),
                    SoilPostId = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilPostData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoilPostData_SoilContaminant_SoilContaminantId",
                        column: x => x.SoilContaminantId,
                        principalTable: "SoilContaminant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoilPostData_SoilPost_SoilPostId",
                        column: x => x.SoilPostId,
                        principalTable: "SoilPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoilPostData_SoilContaminantId",
                table: "SoilPostData",
                column: "SoilContaminantId");

            migrationBuilder.CreateIndex(
                name: "IX_SoilPostData_SoilPostId",
                table: "SoilPostData",
                column: "SoilPostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoilPostData");
        }
    }
}
