using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class GeneralWeatherCondition3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Default",
                table: "GeneralWeatherCondition",
                type: "bool",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Default",
                table: "GeneralWeatherCondition",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bool");
        }
    }
}
