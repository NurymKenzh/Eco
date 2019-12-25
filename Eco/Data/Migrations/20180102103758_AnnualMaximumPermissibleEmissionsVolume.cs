using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class AnnualMaximumPermissibleEmissionsVolume : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnnualMaximumPermissibleEmissionsVolume",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CompanyId = table.Column<int>(type: "int4", nullable: false),
                    DateOfIssueOfPermit = table.Column<DateTime>(type: "timestamp", nullable: false),
                    EmissionsTonsPerYear = table.Column<decimal>(type: "numeric", nullable: false),
                    IssuingPermitsStateAuthorityId = table.Column<int>(type: "int4", nullable: false),
                    SubsidiaryCompanyId = table.Column<int>(type: "int4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnualMaximumPermissibleEmissionsVolume", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnualMaximumPermissibleEmissionsVolume_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnnualMaximumPermissibleEmissionsVolume_IssuingPermitsStateAuthority_IssuingPermitsStateAuthorityId",
                        column: x => x.IssuingPermitsStateAuthorityId,
                        principalTable: "IssuingPermitsStateAuthority",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnnualMaximumPermissibleEmissionsVolume_Company_SubsidiaryCompanyId",
                        column: x => x.SubsidiaryCompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnualMaximumPermissibleEmissionsVolume_CompanyId",
                table: "AnnualMaximumPermissibleEmissionsVolume",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualMaximumPermissibleEmissionsVolume_IssuingPermitsStateAuthorityId",
                table: "AnnualMaximumPermissibleEmissionsVolume",
                column: "IssuingPermitsStateAuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualMaximumPermissibleEmissionsVolume_SubsidiaryCompanyId",
                table: "AnnualMaximumPermissibleEmissionsVolume",
                column: "SubsidiaryCompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnualMaximumPermissibleEmissionsVolume");
        }
    }
}
