using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class AirContaminant5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContaminantCodeERA",
                table: "AirContaminant");

            migrationBuilder.DropColumn(
                name: "Synonyms",
                table: "AirContaminant");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContaminantCodeERA",
                table: "AirContaminant",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "Synonyms",
                table: "AirContaminant",
                nullable: true);
        }
    }
}
