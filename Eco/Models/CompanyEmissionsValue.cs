using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class CompanyEmissionsValue
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EmissionSource")]
        public EmissionSource EmissionSource { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EmissionSource")]
        public int EmissionSourceId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AirContaminant")]
        public AirContaminant AirContaminant { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AirContaminant")]
        public int AirContaminantId { get; set; }

        //[Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CoefficientOfSettlement")]
        //[DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
        //[Range(0, 9.9, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        //public decimal CoefficientOfSettlement { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ValuesMaximumPermissibleEmissionsgs")]
        [DisplayFormat(DataFormatString = "{0:0.000000}", ApplyFormatInEditMode = true)]
        [Range(0, 999999.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? ValuesMaximumPermissibleEmissionsgs { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ValuesMaximumPermissibleEmissionstyear")]
        [DisplayFormat(DataFormatString = "{0:0.000000}", ApplyFormatInEditMode = true)]
        [Range(0, 999999.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? ValuesMaximumPermissibleEmissionstyear { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ValuesMaximumPermissibleEmissionsmgm3")]
        [DisplayFormat(DataFormatString = "{0:0.000000}", ApplyFormatInEditMode = true)]
        [Range(0, 999999.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? ValuesMaximumPermissibleEmissionsmgm3 { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "YearOfAchievementMaximumPermissibleEmissions")]
        [Range(Constants.YearDataMin, Constants.YearMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int? YearOfAchievementMaximumPermissibleEmissions { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CoefficientOfGasCleaningPlanned")]
        [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
        [Range(0, 100, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? CoefficientOfGasCleaningPlanned { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CoefficientOfGasCleaningActual")]
        [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
        [Range(0, 100, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? CoefficientOfGasCleaningActual { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AverageOperatingDegreeOfPurification")]
        [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
        [Range(0, 100, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? AverageOperatingDegreeOfPurification { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MaximumDegreeOfPurification")]
        [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
        [Range(0, 100, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? MaximumDegreeOfPurification { get; set; }

        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"EmissionSourceId: {EmissionSourceId.ToString()}\r\n" +
                $"AirContaminantId: {AirContaminantId.ToString()}\r\n" +
                //$"CoefficientOfSettlement: {CoefficientOfSettlement.ToString()}\r\n" +
                $"ValuesMaximumPermissibleEmissionsgs: {ValuesMaximumPermissibleEmissionsgs.ToString()}\r\n" +
                $"ValuesMaximumPermissibleEmissionstyear: {ValuesMaximumPermissibleEmissionstyear.ToString()}\r\n" +
                $"ValuesMaximumPermissibleEmissionsmgm3: {ValuesMaximumPermissibleEmissionsmgm3.ToString()}\r\n" +
                $"YearOfAchievementMaximumPermissibleEmissions: {YearOfAchievementMaximumPermissibleEmissions.ToString()}\r\n" +
                $"CoefficientOfGasCleaningPlanned: {CoefficientOfGasCleaningPlanned.ToString()}\r\n" +
                $"CoefficientOfGasCleaningActual: {CoefficientOfGasCleaningActual.ToString()}\r\n" +
                $"AverageOperatingDegreeOfPurification: {AverageOperatingDegreeOfPurification.ToString()}\r\n" +
                $"MaximumDegreeOfPurification: {MaximumDegreeOfPurification.ToString()}";
        }
    }

    public class CompanyEmissionsValueIndexPageViewModel
    {
        public IEnumerable<CompanyEmissionsValue> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
