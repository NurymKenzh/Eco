using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class GreenPlantationsState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GreenPlantationsState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AdditionalInformationKK = table.Column<string>(type: "text", nullable: true),
                    AdditionalInformationRU = table.Column<string>(type: "text", nullable: true),
                    Areahectares = table.Column<decimal>(type: "numeric", nullable: false),
                    CityDistrictId = table.Column<int>(type: "int4", nullable: false),
                    GreenPlantationsTypeId = table.Column<int>(type: "int4", nullable: false),
                    NameKK = table.Column<string>(type: "text", nullable: true),
                    NameRU = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GreenPlantationsState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GreenPlantationsState_CityDistrict_CityDistrictId",
                        column: x => x.CityDistrictId,
                        principalTable: "CityDistrict",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GreenPlantationsState_GreenPlantationsType_GreenPlantationsTypeId",
                        column: x => x.GreenPlantationsTypeId,
                        principalTable: "GreenPlantationsType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GreenPlantationsState_CityDistrictId",
                table: "GreenPlantationsState",
                column: "CityDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_GreenPlantationsState_GreenPlantationsTypeId",
                table: "GreenPlantationsState",
                column: "GreenPlantationsTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GreenPlantationsState");
        }
    }
}
