using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class AActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AActivity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ActivityType = table.Column<bool>(type: "bool", nullable: false),
                    AdditionalInformationKK = table.Column<string>(type: "text", nullable: true),
                    AdditionalInformationRU = table.Column<string>(type: "text", nullable: true),
                    EventId = table.Column<int>(type: "int4", nullable: false),
                    ImplementationPercentage = table.Column<decimal>(type: "numeric", nullable: true),
                    TargetId = table.Column<int>(type: "int4", nullable: false),
                    TargetTerritoryId = table.Column<int>(type: "int4", nullable: false),
                    Year = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AActivity_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AActivity_Target_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Target",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AActivity_TargetTerritory_TargetTerritoryId",
                        column: x => x.TargetTerritoryId,
                        principalTable: "TargetTerritory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AActivity_EventId",
                table: "AActivity",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_AActivity_TargetId",
                table: "AActivity",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_AActivity_TargetTerritoryId",
                table: "AActivity",
                column: "TargetTerritoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AActivity");
        }
    }
}
