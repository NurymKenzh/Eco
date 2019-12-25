using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class TargetValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TargetValue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AdditionalInformationKK = table.Column<string>(type: "text", nullable: true),
                    AdditionalInformationRU = table.Column<string>(type: "text", nullable: true),
                    TargetId = table.Column<int>(type: "int4", nullable: false),
                    TargetTerritoryId = table.Column<int>(type: "int4", nullable: false),
                    TargetValueType = table.Column<bool>(type: "bool", nullable: false),
                    Year = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TargetValue_Target_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Target",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TargetValue_TargetTerritory_TargetTerritoryId",
                        column: x => x.TargetTerritoryId,
                        principalTable: "TargetTerritory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TargetValue_TargetId",
                table: "TargetValue",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetValue_TargetTerritoryId",
                table: "TargetValue",
                column: "TargetTerritoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TargetValue");
        }
    }
}
