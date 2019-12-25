using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class WasteRecyclingCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WasteRecyclingCompany",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AdditionalInformationKK = table.Column<string>(type: "text", nullable: true),
                    AdditionalInformationRU = table.Column<string>(type: "text", nullable: true),
                    AddressContactInformation = table.Column<string>(type: "text", nullable: true),
                    BIK = table.Column<string>(type: "text", nullable: true),
                    EastLongitude = table.Column<decimal>(type: "numeric", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    NorthLatitude = table.Column<decimal>(type: "numeric", nullable: false),
                    RecyclableWasteTypeId = table.Column<int>(type: "int4", nullable: false),
                    Status = table.Column<bool>(type: "bool", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WasteRecyclingCompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WasteRecyclingCompany_RecyclableWasteType_RecyclableWasteTypeId",
                        column: x => x.RecyclableWasteTypeId,
                        principalTable: "RecyclableWasteType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WasteRecyclingCompany_RecyclableWasteTypeId",
                table: "WasteRecyclingCompany",
                column: "RecyclableWasteTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WasteRecyclingCompany");
        }
    }
}
