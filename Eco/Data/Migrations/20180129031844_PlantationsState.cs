using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class PlantationsState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlantationsState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CityDistrictId = table.Column<int>(type: "int4", nullable: false),
                    PlantationsStateTypeId = table.Column<int>(type: "int4", nullable: false),
                    TreesNumber = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantationsState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantationsState_CityDistrict_CityDistrictId",
                        column: x => x.CityDistrictId,
                        principalTable: "CityDistrict",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlantationsState_PlantationsStateType_PlantationsStateTypeId",
                        column: x => x.PlantationsStateTypeId,
                        principalTable: "PlantationsStateType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlantationsState_CityDistrictId",
                table: "PlantationsState",
                column: "CityDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantationsState_PlantationsStateTypeId",
                table: "PlantationsState",
                column: "PlantationsStateTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlantationsState");
        }
    }
}
