using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class SpeciesDiversity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpeciesDiversity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CityDistrictId = table.Column<int>(type: "int4", nullable: false),
                    PlantationsTypeId = table.Column<int>(type: "int4", nullable: false),
                    TreesNumber = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeciesDiversity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpeciesDiversity_CityDistrict_CityDistrictId",
                        column: x => x.CityDistrictId,
                        principalTable: "CityDistrict",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpeciesDiversity_PlantationsType_PlantationsTypeId",
                        column: x => x.PlantationsTypeId,
                        principalTable: "PlantationsType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesDiversity_CityDistrictId",
                table: "SpeciesDiversity",
                column: "CityDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesDiversity_PlantationsTypeId",
                table: "SpeciesDiversity",
                column: "PlantationsTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpeciesDiversity");
        }
    }
}
