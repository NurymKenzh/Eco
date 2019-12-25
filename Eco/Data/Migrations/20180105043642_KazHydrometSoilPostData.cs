using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class KazHydrometSoilPostData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KazHydrometSoilPostData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ConcentrationValuemgkg = table.Column<decimal>(type: "numeric", nullable: false),
                    KazHydrometSoilPostId = table.Column<int>(type: "int4", nullable: false),
                    Season = table.Column<bool>(type: "bool", nullable: false),
                    SoilContaminantId = table.Column<int>(type: "int4", nullable: false),
                    Year = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KazHydrometSoilPostData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KazHydrometSoilPostData_KazHydrometSoilPost_KazHydrometSoilPostId",
                        column: x => x.KazHydrometSoilPostId,
                        principalTable: "KazHydrometSoilPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KazHydrometSoilPostData_SoilContaminant_SoilContaminantId",
                        column: x => x.SoilContaminantId,
                        principalTable: "SoilContaminant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KazHydrometSoilPostData_KazHydrometSoilPostId",
                table: "KazHydrometSoilPostData",
                column: "KazHydrometSoilPostId");

            migrationBuilder.CreateIndex(
                name: "IX_KazHydrometSoilPostData_SoilContaminantId",
                table: "KazHydrometSoilPostData",
                column: "SoilContaminantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KazHydrometSoilPostData");
        }
    }
}
