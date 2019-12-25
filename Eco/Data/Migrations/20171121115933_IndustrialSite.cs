using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class IndustrialSite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IndustrialSite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AbbreviatedName = table.Column<string>(type: "text", nullable: true),
                    CityDistrictId = table.Column<int>(type: "int4", nullable: false),
                    CompanyId = table.Column<int>(type: "int4", nullable: false),
                    EastLongitude = table.Column<decimal>(type: "numeric", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    HazardClassId = table.Column<int>(type: "int4", nullable: false),
                    House = table.Column<string>(type: "text", nullable: true),
                    NorthLatitude = table.Column<decimal>(type: "numeric", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: true),
                    SubsidiaryCompanyId = table.Column<int>(type: "int4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndustrialSite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndustrialSite_CityDistrict_CityDistrictId",
                        column: x => x.CityDistrictId,
                        principalTable: "CityDistrict",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndustrialSite_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndustrialSite_HazardClass_HazardClassId",
                        column: x => x.HazardClassId,
                        principalTable: "HazardClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndustrialSite_Company_SubsidiaryCompanyId",
                        column: x => x.SubsidiaryCompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndustrialSite_CityDistrictId",
                table: "IndustrialSite",
                column: "CityDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_IndustrialSite_CompanyId",
                table: "IndustrialSite",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_IndustrialSite_HazardClassId",
                table: "IndustrialSite",
                column: "HazardClassId");

            migrationBuilder.CreateIndex(
                name: "IX_IndustrialSite_SubsidiaryCompanyId",
                table: "IndustrialSite",
                column: "SubsidiaryCompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndustrialSite");
        }
    }
}
