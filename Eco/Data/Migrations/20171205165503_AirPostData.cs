using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class AirPostData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AirPostData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AirPostId = table.Column<int>(type: "int4", nullable: false),
                    AtmosphericPressurekPa = table.Column<decimal>(type: "numeric", nullable: false),
                    Benzapyrene = table.Column<decimal>(type: "numeric", nullable: false),
                    COmgm3 = table.Column<decimal>(type: "numeric", nullable: false),
                    CarbonBlackmgm3 = table.Column<decimal>(type: "numeric", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    GeneralWeatherConditionId = table.Column<int>(type: "int4", nullable: false),
                    Humidity = table.Column<int>(type: "int4", nullable: false),
                    NO2mgm3 = table.Column<decimal>(type: "numeric", nullable: false),
                    NOmg3 = table.Column<decimal>(type: "numeric", nullable: false),
                    SO2mgm3 = table.Column<decimal>(type: "numeric", nullable: false),
                    SuspendedSubstances = table.Column<decimal>(type: "numeric", nullable: false),
                    TemperatureC = table.Column<decimal>(type: "numeric", nullable: false),
                    WindDirectionId = table.Column<int>(type: "int4", nullable: false),
                    WindSpeedms = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirPostData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirPostData_AirPost_AirPostId",
                        column: x => x.AirPostId,
                        principalTable: "AirPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AirPostData_GeneralWeatherCondition_GeneralWeatherConditionId",
                        column: x => x.GeneralWeatherConditionId,
                        principalTable: "GeneralWeatherCondition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AirPostData_WindDirection_WindDirectionId",
                        column: x => x.WindDirectionId,
                        principalTable: "WindDirection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirPostData_AirPostId",
                table: "AirPostData",
                column: "AirPostId");

            migrationBuilder.CreateIndex(
                name: "IX_AirPostData_GeneralWeatherConditionId",
                table: "AirPostData",
                column: "GeneralWeatherConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_AirPostData_WindDirectionId",
                table: "AirPostData",
                column: "WindDirectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirPostData");
        }
    }
}
