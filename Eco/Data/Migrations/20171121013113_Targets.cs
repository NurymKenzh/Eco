using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class Targets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Target",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NameKK = table.Column<string>(type: "text", nullable: true),
                    NameRU = table.Column<string>(type: "text", nullable: true),
                    TypeOfAchievement = table.Column<bool>(type: "bool", nullable: false),
                    TypeOfTargetId = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Target", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Target_TypeOfTarget_TypeOfTargetId",
                        column: x => x.TypeOfTargetId,
                        principalTable: "TypeOfTarget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Target_TypeOfTargetId",
                table: "Target",
                column: "TypeOfTargetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Target");
        }
    }
}
