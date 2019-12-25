using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class CityDistrict2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Area",
                table: "CityDistrict",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "Populations",
                table: "CityDistrict",
                type: "int4[]",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "Years",
                table: "CityDistrict",
                type: "int4[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "CityDistrict");

            migrationBuilder.DropColumn(
                name: "Populations",
                table: "CityDistrict");

            migrationBuilder.DropColumn(
                name: "Years",
                table: "CityDistrict");
        }
    }
}
