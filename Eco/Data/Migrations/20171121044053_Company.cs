using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class Company : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AbbreviatedName = table.Column<string>(type: "text", nullable: true),
                    ActualAddressHouse = table.Column<string>(type: "text", nullable: true),
                    ActualAddressStreet = table.Column<string>(type: "text", nullable: true),
                    AdditionalInformation = table.Column<string>(type: "text", nullable: true),
                    BIK = table.Column<string>(type: "text", nullable: true),
                    CityDistrictId = table.Column<int>(type: "int4", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    HazardClassId = table.Column<int>(type: "int4", nullable: false),
                    HierarchicalStructure = table.Column<bool>(type: "bool", nullable: false),
                    KindOfActivity = table.Column<string>(type: "text", nullable: true),
                    LegalAddressHouse = table.Column<string>(type: "text", nullable: true),
                    LegalAddressStreet = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_CityDistrict_CityDistrictId",
                        column: x => x.CityDistrictId,
                        principalTable: "CityDistrict",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Company_HazardClass_HazardClassId",
                        column: x => x.HazardClassId,
                        principalTable: "HazardClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_CityDistrictId",
                table: "Company",
                column: "CityDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_HazardClassId",
                table: "Company",
                column: "HazardClassId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
