using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class KazHydrometSoilPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KazHydrometSoilPost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AdditionalInformationKK = table.Column<string>(type: "text", nullable: true),
                    AdditionalInformationRU = table.Column<string>(type: "text", nullable: true),
                    EastLongitude = table.Column<decimal>(type: "numeric", nullable: false),
                    NorthLatitude = table.Column<decimal>(type: "numeric", nullable: false),
                    Number = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KazHydrometSoilPost", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KazHydrometSoilPost");
        }
    }
}
