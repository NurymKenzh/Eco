using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class Company2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualAddressHouse",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "ActualAddressStreet",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "LegalAddressHouse",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "LegalAddressStreet",
                table: "Company");

            migrationBuilder.AddColumn<string>(
                name: "ActualAddress",
                table: "Company",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalAddress",
                table: "Company",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualAddress",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "LegalAddress",
                table: "Company");

            migrationBuilder.AddColumn<string>(
                name: "ActualAddressHouse",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActualAddressStreet",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalAddressHouse",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalAddressStreet",
                table: "Company",
                nullable: true);
        }
    }
}
