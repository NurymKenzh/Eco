using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class KazHydrometSoilPost2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameKK",
                table: "KazHydrometSoilPost",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameRU",
                table: "KazHydrometSoilPost",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameKK",
                table: "KazHydrometSoilPost");

            migrationBuilder.DropColumn(
                name: "NameRU",
                table: "KazHydrometSoilPost");
        }
    }
}
