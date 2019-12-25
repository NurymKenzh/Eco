using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class TransportPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransportPost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AdditionalInformationKK = table.Column<string>(type: "text", nullable: true),
                    AdditionalInformationRU = table.Column<string>(type: "text", nullable: true),
                    EastLongitude = table.Column<decimal>(type: "numeric", nullable: false),
                    MovementDirectionId = table.Column<int>(type: "int4", nullable: false),
                    NameKK = table.Column<string>(type: "text", nullable: true),
                    NameRU = table.Column<string>(type: "text", nullable: true),
                    NorthLatitude = table.Column<decimal>(type: "numeric", nullable: false),
                    NumberOfBands = table.Column<int>(type: "int4", nullable: false),
                    Type = table.Column<bool>(type: "bool", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportPost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportPost_MovementDirection_MovementDirectionId",
                        column: x => x.MovementDirectionId,
                        principalTable: "MovementDirection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransportPost_MovementDirectionId",
                table: "TransportPost",
                column: "MovementDirectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransportPost");
        }
    }
}
