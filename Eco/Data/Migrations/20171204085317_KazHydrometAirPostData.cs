using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class KazHydrometAirPostData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KazHydrometAirPostData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AirContaminantId = table.Column<int>(type: "int4", nullable: false),
                    KazHydrometAirPostId = table.Column<int>(type: "int4", nullable: false),
                    Month = table.Column<int>(type: "int4", nullable: false),
                    PollutantConcentrationMaximumOneTimePerMonth = table.Column<decimal>(type: "numeric", nullable: false),
                    PollutantConcentrationMaximumOneTimePerMonthDay = table.Column<int>(type: "int4", nullable: false),
                    PollutantConcentrationMonthlyAverage = table.Column<decimal>(type: "numeric", nullable: false),
                    Year = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KazHydrometAirPostData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KazHydrometAirPostData_AirContaminant_AirContaminantId",
                        column: x => x.AirContaminantId,
                        principalTable: "AirContaminant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KazHydrometAirPostData_KazHydrometAirPost_KazHydrometAirPostId",
                        column: x => x.KazHydrometAirPostId,
                        principalTable: "KazHydrometAirPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KazHydrometAirPostData_AirContaminantId",
                table: "KazHydrometAirPostData",
                column: "AirContaminantId");

            migrationBuilder.CreateIndex(
                name: "IX_KazHydrometAirPostData_KazHydrometAirPostId",
                table: "KazHydrometAirPostData",
                column: "KazHydrometAirPostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KazHydrometAirPostData");
        }
    }
}
