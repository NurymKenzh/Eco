using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class SubsidiaryCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Company",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Company",
                type: "int4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_CompanyId",
                table: "Company",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Company_CompanyId",
                table: "Company",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Company_CompanyId",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_CompanyId",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Company");
        }
    }
}
