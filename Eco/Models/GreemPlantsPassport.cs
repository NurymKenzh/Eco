using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class GreemPlantsPassport
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "GreenObject")]
        public string GreenObject{ get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CityDistrict")]
        public int CityDistrictId { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CityDistrict")]
        public CityDistrict CityDistrict { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NameOfPowersAttributed")]
        public string NameOfPowersAttributed { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NameOfRegistrationObject")]
        public string NameOfRegistrationObject { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "LegalEntityUse")]
        public string LegalEntityUse { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AccountNumber")]
        public string AccountNumber { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NameAndLocation")]
        public string NameAndLocation { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PresenceOfHistoricalObject")]
        public string PresenceOfHistoricalObject{ get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "GreenTotalAreaGa")]
        public decimal? GreenTotalAreaGa { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Lawns")]
        public decimal? Lawns { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Flowerbeds")]
        public decimal? Flowerbeds { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TracksAndPlatforms")]
        public decimal? TracksAndPlatforms { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Tree")]
        public int? Tree { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Shrubs")]
        public int? Shrubs { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SofasAndBenches")]
        public int? SofasAndBenches { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Urns")]
        public int? Urns { get; set; }
                
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EquippedPlaygrounds")]
        public int? EquippedPlaygrounds { get; set; }
                
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EquippedSportsgrounds")]
        public int? EquippedSportsgrounds { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Monument")]
        public int? Monument { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Toilets")]
        public int? Toilets { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "OutdoorLighting")]
        public int? OutdoorLighting { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Billboards")]
        public int? Billboards { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "OtherCapitalStructures")]
        public int? OtherCapitalStructures { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "GreenTotalArea")]
        public decimal? GreenTotalArea { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AreaUnderGreenery")]
        public decimal? AreaUnderGreenery { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AreaUnderLawn")]
        public decimal? AreaUnderLawn { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AreaUnderGroundlawn")]
        public decimal? AreaUnderGroundlawn { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AreaUnderOrdinarylawn")]
        public decimal? AreaUnderOrdinarylawn { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AreaUnderMeadowlawn")]
        public decimal? AreaUnderMeadowlawn { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AreaUnderTrees")]
        public decimal? AreaUnderTrees { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AreaUnderShrubs")]
        public decimal? AreaUnderShrubs { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AreaUndeFlowerbeds")]
        public decimal? AreaUndeFlowerbeds { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AreaUndeTracksAndPlatforms")]
        public decimal? AreaUndeTracksAndPlatforms { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Asphalted")]
        public decimal? Asphalted { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PavingBlocks")]
        public decimal? PavingBlocks { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "LengthOfTrays")]
        public decimal? LengthOfTrays { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AmountConiferousTrees")]
        public int? AmountConiferousTrees { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ListOfTreesConiferous")]
        public int? ListOfTreesConiferous { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Upto10yearsConiferous")]
        public int? Upto10yearsConiferous { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Betwen10_20yearsConiferous")]
        public int? Betwen10_20yearsConiferous { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Over10yearsConiferous")]
        public int? Over10yearsConiferous { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AmountDeciduousTrees")]
        public int? AmountDeciduousTrees { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ListOfTreesDeciduous")]
        public int? ListOfTreesDeciduous { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Upto10yearsDeciduous")]
        public int? Upto10yearsDeciduous { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Betwen10_20yearsDeciduous")]
        public int? Betwen10_20yearsDeciduous { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Over10yearsDeciduous")]
        public int? Over10yearsDeciduous { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AmountFormedTrees")]
        public int? AmountFormedTrees { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TotallAmountShrubs")]
        public int? TotallAmountShrubs { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AmountShrubs")]
        public int? AmountShrubs { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "LengthOfHedges")]
        public int? LengthOfHedges { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AmountEquippedPlaygrounds")]
        public int? AmountEquippedPlaygrounds { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AmountEquippedSportsgrounds")]
        public int? AmountEquippedSportsgrounds { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AmountSofasAndBenches")]
        public int? AmountSofasAndBenches { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AmountBenches")]
        public int? AmountBenches { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AmountSofas")]
        public int? AmountSofas { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AmountArbours")]
        public int? AmountArbours { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AmountOutdoorLighting")]
        public int? AmountOutdoorLighting { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AmountToilets")]
        public int? AmountToilets { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AmountMonument")]
        public int? AmountMonument { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AmountBillboards")]
        public int? AmountBillboards { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ListOfTreesByObjectBreedsCondition")]
        public int? ListOfTreesByObjectBreedsCondition { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ListOfTreesByObjectEconomicMeasures")]
        public int? ListOfTreesByObjectEconomicMeasures { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PassportGeneralInformation")]
        public string PassportGeneralInformation { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NorthLatitude")]
        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNeedToInput")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        [Range(0, 99.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal NorthLatitude { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EastLongitude")]
        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNeedToInput")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        [Range(0, 99.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal EastLongitude { get; set; }
        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"GreenObject: {GreenObject}\r\n" +
                $"NameOfAdmDistrict: {CityDistrictId.ToString()}\r\n" +
                $"NameOfPowersAttributed: {NameOfPowersAttributed}\r\n" +
                $"NameOfRegistrationObject: {NameOfRegistrationObject}\r\n" +
                $"LegalEntityUse: {LegalEntityUse}\r\n" +
                $"AccountNumber: {AccountNumber}\r\n" +
                $"NameAndLocation: {NameAndLocation}\r\n" +
                $"PresenceOfHistoricalObject : {PresenceOfHistoricalObject}\r\n" +
                $"GreenTotalArea: {GreenTotalArea}\r\n" +
                $"Lawns: {Lawns}\r\n" +
                $"Flowerbeds : {Flowerbeds}\r\n" +
                $"TracksAndPlatforms: {TracksAndPlatforms}\r\n" +
                $"Tree: {Tree}\r\n" +
                $"Shrubs: {Shrubs}\r\n" +
                $"SofasAndBenches : {SofasAndBenches}\r\n" +
                $"LegalEntityUse: {LegalEntityUse}\r\n" +
                $"Urns: {Urns}\r\n" +
                $"EquippedPlaygrounds: {EquippedPlaygrounds}\r\n" +
                $"EquippedSportsgrounds : {EquippedSportsgrounds}\r\n" +
                $"Monument : {Monument}\r\n" +
                $"Toilets : {Toilets}\r\n" +
                $"OutdoorLighting : {OutdoorLighting}\r\n" +
                $"Billboards : {Billboards}\r\n" +
                $"OtherCapitalStructures : {OtherCapitalStructures}\r\n" +
                $"GreenTotalArea : {GreenTotalArea}\r\n" +
                $"AreaUnderGreenery : {AreaUnderGreenery}\r\n" +
                $"EquippedSportsgrounds : {AreaUnderLawn}\r\n" +
                $"AreaUnderLawn : {AreaUnderLawn}\r\n" +
                $"AreaUnderGroundlawn : {AreaUnderGroundlawn}\r\n" +
                $"AreaUnderOrdinarylawn : {AreaUnderOrdinarylawn}\r\n" +
                $"AreaUnderMeadowlawn : {AreaUnderMeadowlawn}\r\n" +
                $"AreaUnderTrees : {AreaUnderTrees}\r\n" +
                $"AreaUnderShrubs : {AreaUnderShrubs}\r\n" +
                $"AreaUndeFlowerbeds : {AreaUndeFlowerbeds}\r\n" +
                $"AreaUndeTracksAndPlatforms : {AreaUndeTracksAndPlatforms}\r\n" +
                $"Asphalted : {Asphalted}\r\n" +
                $"PavingBlocks : {PavingBlocks}\r\n" +
                $"LengthOfTrays : {LengthOfTrays}\r\n" +
                $"AmountConiferousTrees : {AmountConiferousTrees}\r\n" +
                $"ListOfTreesConiferous : {ListOfTreesConiferous}\r\n" +
                $"Upto10yearsConiferous : {Upto10yearsConiferous}\r\n" +
                $"Betwen10_20yearsConiferous : {Betwen10_20yearsConiferous}\r\n" +
                $"Over10yearsConiferous : {Over10yearsConiferous}\r\n" +
                $"AmountDeciduousTrees : {AmountDeciduousTrees}\r\n" +
                $"ListOfTreesDeciduous : {ListOfTreesDeciduous}\r\n" +
                $"Upto10yearsDeciduous : {Upto10yearsDeciduous}\r\n" +
                $"Betwen10_20yearsDeciduous : {Betwen10_20yearsDeciduous}\r\n" +
                $"Over10yearsDeciduous : {Over10yearsDeciduous}\r\n" +
                $"AmountFormedTrees : {AmountFormedTrees}\r\n" +
                $"TotallAmountShrubs : {TotallAmountShrubs}\r\n" +
                $"AmountShrubs : {AmountShrubs}\r\n" +
                $"LengthOfHedges : {LengthOfHedges}\r\n" +
                $"AmountEquippedPlaygrounds : {AmountEquippedPlaygrounds}\r\n" +
                $"AmountEquippedSportsgrounds : {AmountEquippedSportsgrounds}\r\n" +
                $"AmountSofasAndBenches : {AmountSofasAndBenches}\r\n" +
                $"AmountBenches : {AmountBenches}\r\n" +
                $"AmountSofas : {AmountSofas}\r\n" +
                $"AmountArbours : {AmountArbours}\r\n" +
                $"AmountOutdoorLighting : {AmountOutdoorLighting}\r\n" +
                $"AmountToilets : {AmountToilets}\r\n" +
                $"AmountMonument : {AmountMonument}\r\n" +
                $"AmountBillboards : {AmountBillboards}\r\n" +
                $"ListOfTreesByObjectBreedsCondition : {ListOfTreesByObjectBreedsCondition}\r\n" +
                $"ListOfTreesByObjectEconomicMeasures : {ListOfTreesByObjectEconomicMeasures}\r\n" +
                $"PassportGeneralInformation : {PassportGeneralInformation}\r\n" +
                $"NorthLatitude : {NorthLatitude}\r\n" +
                $"EastLongitude : {EastLongitude}";
        }
    }


    public class GreemPlantsPassportIndexPageViewModel
    {
        public IEnumerable<GreemPlantsPassport> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
