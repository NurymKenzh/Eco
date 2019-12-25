using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class EmissionSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmissionSource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CompanyId = table.Column<int>(type: "int4", nullable: false),
                    DiameterOfMouthOfPipesOrWidth = table.Column<decimal>(type: "numeric", nullable: true),
                    EastLongitude1 = table.Column<decimal>(type: "numeric", nullable: false),
                    EastLongitude2 = table.Column<decimal>(type: "numeric", nullable: true),
                    EastLongitude3 = table.Column<decimal>(type: "numeric", nullable: true),
                    EastLongitude4 = table.Column<decimal>(type: "numeric", nullable: true),
                    EmissionSourceHeight = table.Column<int>(type: "int4", nullable: false),
                    EmissionSourceMapNumber = table.Column<int>(type: "int4", nullable: false),
                    EmissionSourceName = table.Column<string>(type: "text", nullable: true),
                    EmissionSourceTypeId = table.Column<int>(type: "int4", nullable: false),
                    LengthOfMouth = table.Column<decimal>(type: "numeric", nullable: true),
                    NameOfGasTreatmentPlantsTypeAndMeasuresToReduceEmissions = table.Column<string>(type: "text", nullable: true),
                    NorthLatitude1 = table.Column<decimal>(type: "numeric", nullable: false),
                    NorthLatitude2 = table.Column<decimal>(type: "numeric", nullable: true),
                    NorthLatitude3 = table.Column<decimal>(type: "numeric", nullable: true),
                    NorthLatitude4 = table.Column<decimal>(type: "numeric", nullable: true),
                    SourcesNumber = table.Column<int>(type: "int4", nullable: false),
                    SpeedOfGasAirMixture = table.Column<decimal>(type: "numeric", nullable: true),
                    SubsidiaryCompanyId = table.Column<int>(type: "int4", nullable: true),
                    TemperatureOfMixture = table.Column<decimal>(type: "numeric", nullable: false),
                    VolumeOfGasAirMixture = table.Column<decimal>(type: "numeric", nullable: true),
                    WorkHoursPerYear = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmissionSource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmissionSource_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmissionSource_EmissionSourceType_EmissionSourceTypeId",
                        column: x => x.EmissionSourceTypeId,
                        principalTable: "EmissionSourceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmissionSource_Company_SubsidiaryCompanyId",
                        column: x => x.SubsidiaryCompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmissionSource_CompanyId",
                table: "EmissionSource",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EmissionSource_EmissionSourceTypeId",
                table: "EmissionSource",
                column: "EmissionSourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmissionSource_SubsidiaryCompanyId",
                table: "EmissionSource",
                column: "SubsidiaryCompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmissionSource");
        }
    }
}
