using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class TargetTerritory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TargetTerritory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AdditionalInformationKK = table.Column<string>(type: "text", nullable: true),
                    AdditionalInformationRU = table.Column<string>(type: "text", nullable: true),
                    CityDistrictId = table.Column<int>(type: "int4", nullable: false),
                    GISConnectionCode = table.Column<string>(type: "text", nullable: true),
                    TerritoryNameKK = table.Column<string>(type: "text", nullable: true),
                    TerritoryNameRU = table.Column<string>(type: "text", nullable: true),
                    TerritoryTypeId = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetTerritory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TargetTerritory_CityDistrict_CityDistrictId",
                        column: x => x.CityDistrictId,
                        principalTable: "CityDistrict",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TargetTerritory_TerritoryType_TerritoryTypeId",
                        column: x => x.TerritoryTypeId,
                        principalTable: "TerritoryType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TargetTerritory_CityDistrictId",
                table: "TargetTerritory",
                column: "CityDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetTerritory_TerritoryTypeId",
                table: "TargetTerritory",
                column: "TerritoryTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TargetTerritory");
        }
    }
}
