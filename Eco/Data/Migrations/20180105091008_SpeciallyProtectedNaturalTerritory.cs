using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class SpeciallyProtectedNaturalTerritory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpeciallyProtectedNaturalTerritory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Areahectares = table.Column<int>(type: "int4", nullable: false),
                    AuthorizedAuthorityId = table.Column<int>(type: "int4", nullable: false),
                    NameKK = table.Column<string>(type: "text", nullable: true),
                    NameRU = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeciallyProtectedNaturalTerritory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpeciallyProtectedNaturalTerritory_AuthorizedAuthority_AuthorizedAuthorityId",
                        column: x => x.AuthorizedAuthorityId,
                        principalTable: "AuthorizedAuthority",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpeciallyProtectedNaturalTerritory_AuthorizedAuthorityId",
                table: "SpeciallyProtectedNaturalTerritory",
                column: "AuthorizedAuthorityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpeciallyProtectedNaturalTerritory");
        }
    }
}
