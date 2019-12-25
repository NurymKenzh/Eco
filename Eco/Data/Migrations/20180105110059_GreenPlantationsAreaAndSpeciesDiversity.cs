using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class GreenPlantationsAreaAndSpeciesDiversity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GreenPlantationsAreaAndSpeciesDiversity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AdditionalInformationKK = table.Column<string>(type: "text", nullable: true),
                    AdditionalInformationRU = table.Column<string>(type: "text", nullable: true),
                    AreaOfGreenCommonAreas = table.Column<decimal>(type: "numeric", nullable: false),
                    AreaOfGreenPlantationsOfLimitedUse = table.Column<decimal>(type: "numeric", nullable: false),
                    AreaOfGreenPlantationsOfSpecialUse = table.Column<decimal>(type: "numeric", nullable: false),
                    CityDistrictId = table.Column<int>(type: "int4", nullable: false),
                    NumberOfTreeSpecies = table.Column<int>(type: "int4", nullable: false),
                    Year = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GreenPlantationsAreaAndSpeciesDiversity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GreenPlantationsAreaAndSpeciesDiversity_CityDistrict_CityDistrictId",
                        column: x => x.CityDistrictId,
                        principalTable: "CityDistrict",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GreenPlantationsAreaAndSpeciesDiversity_CityDistrictId",
                table: "GreenPlantationsAreaAndSpeciesDiversity",
                column: "CityDistrictId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GreenPlantationsAreaAndSpeciesDiversity");
        }
    }
}
