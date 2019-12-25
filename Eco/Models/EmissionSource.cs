using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class EmissionSource
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Company")]
        public Company Company { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Company")]
        public int CompanyId { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SubsidiaryCompany")]
        public SubsidiaryCompany SubsidiaryCompany { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SubsidiaryCompany")]
        public int? SubsidiaryCompanyId { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Company")]
        public string CompanyOrSubsidiaryCompanyAbbreviatedName
        {
            get
            {
                string CompanyOrSubsidiaryCompanyAbbreviatedName = Company != null ? $"{Company.AbbreviatedName}" : "";
                CompanyOrSubsidiaryCompanyAbbreviatedName +=
                    CompanyOrSubsidiaryCompanyAbbreviatedName != "" ?
                    (SubsidiaryCompany != null ? $" ({SubsidiaryCompany.AbbreviatedName})" : "") :
                    (SubsidiaryCompany != null ? $"{SubsidiaryCompany.AbbreviatedName}" : "");
                return CompanyOrSubsidiaryCompanyAbbreviatedName;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EmissionSourceName")]
        public string EmissionSourceName { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EmissionSourceMapNumber")]
        [Range(0, 9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int EmissionSourceMapNumber { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WorkHoursPerYear")]
        [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
        [Range(0, 999999.9, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? WorkHoursPerYear { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SourcesNumber")]
        [Range(0, 99, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int? SourcesNumber { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EmissionSourceType")]
        public EmissionSourceType EmissionSourceType { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EmissionSourceType")]
        public int EmissionSourceTypeId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NorthLatitude")]
        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNeedToInput")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        [Range(0, 99.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal NorthLatitude1 { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EastLongitude")]
        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNeedToInput")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        [Range(0, 99.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal EastLongitude1 { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NorthLatitude")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        [Range(0, 99.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? NorthLatitude2 { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EastLongitude")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        [Range(0, 99.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? EastLongitude2 { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NorthLatitude")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        [Range(0, 99.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? NorthLatitude3 { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EastLongitude")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        [Range(0, 99.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? EastLongitude3 { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NorthLatitude")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        [Range(0, 99.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? NorthLatitude4 { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EastLongitude")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        [Range(0, 99.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? EastLongitude4 { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EmissionSourceHeight")]
        [Range(0, 99, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? EmissionSourceHeight { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "LengthOfMouth")]
        [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
        [Range(0, 99.9, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? LengthOfMouth { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "DiameterOfMouthOfPipesOrWidth")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        [Range(0, 99.9, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? DiameterOfMouthOfPipesOrWidth { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SpeedOfGasAirMixture")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        public decimal? SpeedOfGasAirMixture { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "VolumeOfGasAirMixture")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        public decimal? VolumeOfGasAirMixture { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TemperatureOfMixture")]
        [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
        [Range(0, 999.9, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? TemperatureOfMixture { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NameOfGasTreatmentPlantsTypeAndMeasuresToReduceEmissions")]
        public string NameOfGasTreatmentPlantsTypeAndMeasuresToReduceEmissions { get; set; }

        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"CompanyId: {CompanyId.ToString()}\r\n" +
                $"SubsidiaryCompanyId: {SubsidiaryCompanyId.ToString()}\r\n" +
                $"EmissionSourceName: {EmissionSourceName}\r\n" +
                $"EmissionSourceMapNumber: {EmissionSourceMapNumber.ToString()}\r\n" +
                $"WorkHoursPerYear: {WorkHoursPerYear.ToString()}\r\n" +
                $"SourcesNumber: {SourcesNumber.ToString()}\r\n" +
                $"EmissionSourceTypeId: {EmissionSourceTypeId.ToString()}\r\n" +
                $"NorthLatitude1: {NorthLatitude1.ToString()}\r\n" +
                $"EastLongitude1: {EastLongitude1.ToString()}\r\n" +
                $"NorthLatitude2: {NorthLatitude2.ToString()}\r\n" +
                $"EastLongitude2: {EastLongitude2.ToString()}\r\n" +
                $"NorthLatitude3: {NorthLatitude3.ToString()}\r\n" +
                $"EastLongitude3: {EastLongitude3.ToString()}\r\n" +
                $"NorthLatitude4: {NorthLatitude4.ToString()}\r\n" +
                $"EastLongitude4: {EastLongitude4.ToString()}\r\n" +
                $"EmissionSourceHeight: {EmissionSourceHeight.ToString()}\r\n" +
                $"LengthOfMouth: {LengthOfMouth.ToString()}\r\n" +
                $"DiameterOfMouthOfPipesOrWidth: {DiameterOfMouthOfPipesOrWidth.ToString()}\r\n" +
                $"SpeedOfGasAirMixture: {SpeedOfGasAirMixture.ToString()}\r\n" +
                $"VolumeOfGasAirMixture: {VolumeOfGasAirMixture.ToString()}\r\n" +
                $"TemperatureOfMixture: {TemperatureOfMixture.ToString()}\r\n" +
                $"NameOfGasTreatmentPlantsTypeAndMeasuresToReduceEmissions: {NameOfGasTreatmentPlantsTypeAndMeasuresToReduceEmissions}";
        }
    }

    public class EmissionSourceIndexPageViewModel
    {
        public IEnumerable<EmissionSource> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
