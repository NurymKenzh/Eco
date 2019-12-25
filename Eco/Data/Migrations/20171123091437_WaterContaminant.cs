using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class WaterContaminant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WaterContaminant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    LimitingIndicatorId = table.Column<int>(type: "int4", nullable: true),
                    MaximumPermissibleConcentrationWater = table.Column<decimal>(type: "numeric", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    NumberCAS = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    SubstanceHazardClassId = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterContaminant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterContaminant_LimitingIndicator_LimitingIndicatorId",
                        column: x => x.LimitingIndicatorId,
                        principalTable: "LimitingIndicator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WaterContaminant_SubstanceHazardClass_SubstanceHazardClassId",
                        column: x => x.SubstanceHazardClassId,
                        principalTable: "SubstanceHazardClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaterContaminant_LimitingIndicatorId",
                table: "WaterContaminant",
                column: "LimitingIndicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterContaminant_SubstanceHazardClassId",
                table: "WaterContaminant",
                column: "SubstanceHazardClassId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaterContaminant");
        }
    }
}
