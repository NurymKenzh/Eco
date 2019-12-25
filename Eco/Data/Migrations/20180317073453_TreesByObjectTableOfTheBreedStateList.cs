using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class TreesByObjectTableOfTheBreedStateList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TreesByObjectTableOfTheBreedStateList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    GreemPlantsPassportId = table.Column<int>(type: "int4", nullable: false),
                    PlantationsTypeId = table.Column<int>(type: "int4", nullable: false),
                    Quantity = table.Column<string>(type: "text", nullable: true),
                    StateOfCSR15PlantationsTypeId = table.Column<int>(type: "int4", nullable: true),
                    StateOfCSR15_1 = table.Column<int>(type: "int4", nullable: true),
                    StateOfCSR15_2 = table.Column<int>(type: "int4", nullable: true),
                    StateOfCSR15_3 = table.Column<int>(type: "int4", nullable: true),
                    StateOfCSR15_4 = table.Column<int>(type: "int4", nullable: true),
                    StateOfCSR15_5 = table.Column<int>(type: "int4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreesByObjectTableOfTheBreedStateList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreesByObjectTableOfTheBreedStateList_GreemPlantsPassport_GreemPlantsPassportId",
                        column: x => x.GreemPlantsPassportId,
                        principalTable: "GreemPlantsPassport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreesByObjectTableOfTheBreedStateList_PlantationsType_PlantationsTypeId",
                        column: x => x.PlantationsTypeId,
                        principalTable: "PlantationsType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreesByObjectTableOfTheBreedStateList_PlantationsType_StateOfCSR15PlantationsTypeId",
                        column: x => x.StateOfCSR15PlantationsTypeId,
                        principalTable: "PlantationsType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreesByObjectTableOfTheBreedStateList_GreemPlantsPassportId",
                table: "TreesByObjectTableOfTheBreedStateList",
                column: "GreemPlantsPassportId");

            migrationBuilder.CreateIndex(
                name: "IX_TreesByObjectTableOfTheBreedStateList_PlantationsTypeId",
                table: "TreesByObjectTableOfTheBreedStateList",
                column: "PlantationsTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TreesByObjectTableOfTheBreedStateList_StateOfCSR15PlantationsTypeId",
                table: "TreesByObjectTableOfTheBreedStateList",
                column: "StateOfCSR15PlantationsTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreesByObjectTableOfTheBreedStateList");
        }
    }
}
