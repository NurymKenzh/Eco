using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eco.Data.Migrations
{
    public partial class GreemPlantsPassport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GreemPlantsPassport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AccountNumber = table.Column<string>(type: "text", nullable: true),
                    AmountArbours = table.Column<int>(type: "int4", nullable: true),
                    AmountBenches = table.Column<int>(type: "int4", nullable: true),
                    AmountBillboards = table.Column<int>(type: "int4", nullable: true),
                    AmountConiferousTrees = table.Column<int>(type: "int4", nullable: true),
                    AmountDeciduousTrees = table.Column<int>(type: "int4", nullable: true),
                    AmountEquippedPlaygrounds = table.Column<int>(type: "int4", nullable: true),
                    AmountEquippedSportsgrounds = table.Column<int>(type: "int4", nullable: true),
                    AmountFormedTrees = table.Column<int>(type: "int4", nullable: true),
                    AmountMonument = table.Column<int>(type: "int4", nullable: true),
                    AmountOutdoorLighting = table.Column<int>(type: "int4", nullable: true),
                    AmountShrubs = table.Column<int>(type: "int4", nullable: true),
                    AmountSofas = table.Column<int>(type: "int4", nullable: true),
                    AmountSofasAndBenches = table.Column<int>(type: "int4", nullable: true),
                    AmountToilets = table.Column<int>(type: "int4", nullable: true),
                    AreaUndeFlowerbeds = table.Column<decimal>(type: "numeric", nullable: true),
                    AreaUndeTracksAndPlatforms = table.Column<decimal>(type: "numeric", nullable: true),
                    AreaUnderGreenery = table.Column<decimal>(type: "numeric", nullable: true),
                    AreaUnderGroundlawn = table.Column<decimal>(type: "numeric", nullable: true),
                    AreaUnderLawn = table.Column<decimal>(type: "numeric", nullable: true),
                    AreaUnderMeadowlawn = table.Column<decimal>(type: "numeric", nullable: true),
                    AreaUnderOrdinarylawn = table.Column<decimal>(type: "numeric", nullable: true),
                    AreaUnderShrubs = table.Column<decimal>(type: "numeric", nullable: true),
                    AreaUnderTrees = table.Column<decimal>(type: "numeric", nullable: true),
                    Asphalted = table.Column<decimal>(type: "numeric", nullable: true),
                    Betwen10_20yearsConiferous = table.Column<int>(type: "int4", nullable: true),
                    Betwen10_20yearsDeciduous = table.Column<int>(type: "int4", nullable: true),
                    Billboards = table.Column<int>(type: "int4", nullable: true),
                    CityDistrictId = table.Column<int>(type: "int4", nullable: false),
                    EastLongitude = table.Column<decimal>(type: "numeric", nullable: false),
                    EquippedPlaygrounds = table.Column<int>(type: "int4", nullable: true),
                    EquippedSportsgrounds = table.Column<int>(type: "int4", nullable: true),
                    Flowerbeds = table.Column<decimal>(type: "numeric", nullable: true),
                    GreenObject = table.Column<string>(type: "text", nullable: true),
                    GreenTotalArea = table.Column<decimal>(type: "numeric", nullable: true),
                    GreenTotalAreaGa = table.Column<decimal>(type: "numeric", nullable: true),
                    Lawns = table.Column<decimal>(type: "numeric", nullable: true),
                    LegalEntityUse = table.Column<string>(type: "text", nullable: true),
                    LengthOfHedges = table.Column<int>(type: "int4", nullable: true),
                    LengthOfTrays = table.Column<decimal>(type: "numeric", nullable: true),
                    ListOfTreesByObjectBreedsCondition = table.Column<int>(type: "int4", nullable: true),
                    ListOfTreesByObjectEconomicMeasures = table.Column<int>(type: "int4", nullable: true),
                    ListOfTreesConiferous = table.Column<int>(type: "int4", nullable: true),
                    ListOfTreesDeciduous = table.Column<int>(type: "int4", nullable: true),
                    Monument = table.Column<int>(type: "int4", nullable: true),
                    NameAndLocation = table.Column<string>(type: "text", nullable: true),
                    NameOfPowersAttributed = table.Column<string>(type: "text", nullable: true),
                    NameOfRegistrationObject = table.Column<string>(type: "text", nullable: true),
                    NorthLatitude = table.Column<decimal>(type: "numeric", nullable: false),
                    OtherCapitalStructures = table.Column<int>(type: "int4", nullable: true),
                    OutdoorLighting = table.Column<int>(type: "int4", nullable: true),
                    Over10yearsConiferous = table.Column<int>(type: "int4", nullable: true),
                    Over10yearsDeciduous = table.Column<int>(type: "int4", nullable: true),
                    PassportGeneralInformation = table.Column<string>(type: "text", nullable: true),
                    PavingBlocks = table.Column<decimal>(type: "numeric", nullable: true),
                    PresenceOfHistoricalObject = table.Column<string>(type: "text", nullable: true),
                    Shrubs = table.Column<int>(type: "int4", nullable: true),
                    SofasAndBenches = table.Column<int>(type: "int4", nullable: true),
                    Toilets = table.Column<int>(type: "int4", nullable: true),
                    TotallAmountShrubs = table.Column<int>(type: "int4", nullable: true),
                    TracksAndPlatforms = table.Column<decimal>(type: "numeric", nullable: true),
                    Tree = table.Column<int>(type: "int4", nullable: true),
                    Upto10yearsConiferous = table.Column<int>(type: "int4", nullable: true),
                    Upto10yearsDeciduous = table.Column<int>(type: "int4", nullable: true),
                    Urns = table.Column<int>(type: "int4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GreemPlantsPassport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GreemPlantsPassport_CityDistrict_CityDistrictId",
                        column: x => x.CityDistrictId,
                        principalTable: "CityDistrict",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GreemPlantsPassport_CityDistrictId",
                table: "GreemPlantsPassport",
                column: "CityDistrictId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GreemPlantsPassport");
        }
    }
}
