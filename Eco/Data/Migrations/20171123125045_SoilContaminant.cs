using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class SoilContaminant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SoilContaminant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    LimitingSoilIndicatorId = table.Column<int>(type: "int4", nullable: true),
                    MaximumPermissibleConcentrationSoil = table.Column<decimal>(type: "numeric", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilContaminant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoilContaminant_LimitingSoilIndicator_LimitingSoilIndicatorId",
                        column: x => x.LimitingSoilIndicatorId,
                        principalTable: "LimitingSoilIndicator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoilContaminant_LimitingSoilIndicatorId",
                table: "SoilContaminant",
                column: "LimitingSoilIndicatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoilContaminant");
        }
    }
}
