using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class TransportPostData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransportPostData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AverageSpeedkmh = table.Column<int>(type: "int4", nullable: true),
                    DateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    RunningLengthm = table.Column<int>(type: "int4", nullable: true),
                    TheLengthOfTheInhibitorySignalSec = table.Column<int>(type: "int4", nullable: true),
                    TotalNumberOfVehiclesIn20Minutes = table.Column<int>(type: "int4", nullable: false),
                    TransportPostId = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportPostData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportPostData_TransportPost_TransportPostId",
                        column: x => x.TransportPostId,
                        principalTable: "TransportPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransportPostData_TransportPostId",
                table: "TransportPostData",
                column: "TransportPostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransportPostData");
        }
    }
}
