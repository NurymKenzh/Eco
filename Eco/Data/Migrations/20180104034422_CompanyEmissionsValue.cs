using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class CompanyEmissionsValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyEmissionsValue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AirContaminantId = table.Column<int>(type: "int4", nullable: false),
                    AverageOperatingDegreeOfPurification = table.Column<decimal>(type: "numeric", nullable: true),
                    CoefficientOfGasCleaningActual = table.Column<decimal>(type: "numeric", nullable: true),
                    CoefficientOfGasCleaningPlanned = table.Column<decimal>(type: "numeric", nullable: true),
                    CoefficientOfSettlement = table.Column<decimal>(type: "numeric", nullable: false),
                    EmissionSourceId = table.Column<int>(type: "int4", nullable: false),
                    MaximumDegreeOfPurification = table.Column<decimal>(type: "numeric", nullable: true),
                    ValuesMaximumPermissibleEmissionsgs = table.Column<decimal>(type: "numeric", nullable: false),
                    ValuesMaximumPermissibleEmissionsmgm3 = table.Column<decimal>(type: "numeric", nullable: false),
                    ValuesMaximumPermissibleEmissionstyear = table.Column<decimal>(type: "numeric", nullable: false),
                    YearOfAchievementMaximumPermissibleEmissions = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyEmissionsValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyEmissionsValue_AirContaminant_AirContaminantId",
                        column: x => x.AirContaminantId,
                        principalTable: "AirContaminant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyEmissionsValue_EmissionSource_EmissionSourceId",
                        column: x => x.EmissionSourceId,
                        principalTable: "EmissionSource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyEmissionsValue_AirContaminantId",
                table: "CompanyEmissionsValue",
                column: "AirContaminantId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyEmissionsValue_EmissionSourceId",
                table: "CompanyEmissionsValue",
                column: "EmissionSourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyEmissionsValue");
        }
    }
}
