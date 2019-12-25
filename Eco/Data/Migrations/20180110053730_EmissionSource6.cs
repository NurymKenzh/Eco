using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class EmissionSource6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SourcesNumber",
                table: "EmissionSource",
                type: "int4",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SourcesNumber",
                table: "EmissionSource",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4",
                oldNullable: true);
        }
    }
}
