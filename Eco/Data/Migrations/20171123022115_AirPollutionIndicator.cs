using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class AirPollutionIndicator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AirPollutionIndicator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    TypeOfAirPollutionIndicatorId = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirPollutionIndicator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirPollutionIndicator_TypeOfAirPollutionIndicator_TypeOfAirPollutionIndicatorId",
                        column: x => x.TypeOfAirPollutionIndicatorId,
                        principalTable: "TypeOfAirPollutionIndicator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirPollutionIndicator_TypeOfAirPollutionIndicatorId",
                table: "AirPollutionIndicator",
                column: "TypeOfAirPollutionIndicatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirPollutionIndicator");
        }
    }
}
