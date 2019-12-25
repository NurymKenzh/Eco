using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class TreesByFacilityManagementMeasuresList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TreesByFacilityManagementMeasuresList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    BusinessEventsPlantationsTypeId = table.Column<int>(type: "int4", nullable: true),
                    CrownFormation = table.Column<int>(type: "int4", nullable: true),
                    GreemPlantsPassportId = table.Column<int>(type: "int4", nullable: false),
                    MaintenanceWork = table.Column<string>(type: "text", nullable: true),
                    PlantationsTypeId = table.Column<int>(type: "int4", nullable: false),
                    Quantity = table.Column<string>(type: "text", nullable: true),
                    SanitaryFelling = table.Column<int>(type: "int4", nullable: true),
                    SanitaryPruning = table.Column<int>(type: "int4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreesByFacilityManagementMeasuresList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreesByFacilityManagementMeasuresList_PlantationsType_BusinessEventsPlantationsTypeId",
                        column: x => x.BusinessEventsPlantationsTypeId,
                        principalTable: "PlantationsType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TreesByFacilityManagementMeasuresList_GreemPlantsPassport_GreemPlantsPassportId",
                        column: x => x.GreemPlantsPassportId,
                        principalTable: "GreemPlantsPassport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreesByFacilityManagementMeasuresList_PlantationsType_PlantationsTypeId",
                        column: x => x.PlantationsTypeId,
                        principalTable: "PlantationsType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreesByFacilityManagementMeasuresList_BusinessEventsPlantationsTypeId",
                table: "TreesByFacilityManagementMeasuresList",
                column: "BusinessEventsPlantationsTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TreesByFacilityManagementMeasuresList_GreemPlantsPassportId",
                table: "TreesByFacilityManagementMeasuresList",
                column: "GreemPlantsPassportId");

            migrationBuilder.CreateIndex(
                name: "IX_TreesByFacilityManagementMeasuresList_PlantationsTypeId",
                table: "TreesByFacilityManagementMeasuresList",
                column: "PlantationsTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreesByFacilityManagementMeasuresList");
        }
    }
}
