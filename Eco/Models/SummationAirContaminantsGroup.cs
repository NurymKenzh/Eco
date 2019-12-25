using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class SummationAirContaminantsGroup
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SummationGroupCodeERA")]
        [StringLength(2, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorStringLengthMustBe", MinimumLength = 1)]
        public string SummationGroupCodeERA { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Number25012012")]
        [Range(0, 9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Number25012012 { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CoefficientOfPotentiation")]
        [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
        [Range(0, 9.9, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal CoefficientOfPotentiation { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PresenceOfTheMaximumPermissibleConcentration")]
        public bool PresenceOfTheMaximumPermissibleConcentration { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MaximumPermissibleConcentrationOneTimemaximum")]
        [DisplayFormat(DataFormatString = "{0:0.000000}", ApplyFormatInEditMode = true)]
        [Range(0, 9.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? MaximumPermissibleConcentrationOneTimemaximum { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MaximumPermissibleConcentrationDailyAverage")]
        [DisplayFormat(DataFormatString = "{0:0.000000}", ApplyFormatInEditMode = true)]
        [Range(0, 9.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? MaximumPermissibleConcentrationDailyAverage { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ApproximateSafeExposureLevel")]
        [DisplayFormat(DataFormatString = "{0:0.000000}", ApplyFormatInEditMode = true)]
        [Range(0, 9.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? ApproximateSafeExposureLevel { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SubstanceHazardClass")]
        public SubstanceHazardClass SubstanceHazardClass { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SubstanceHazardClass")]
        public int SubstanceHazardClassId { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "LimitingIndicator")]
        public LimitingIndicator LimitingIndicator { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "LimitingIndicator")]
        public int? LimitingIndicatorId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AirContaminants")]
        public int[] AirContaminants { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AirContaminants")]
        [NotMapped]
        public List<AirContaminant> AirContaminantsList { get; set; }
        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"SummationGroupCodeERA: {SummationGroupCodeERA}\r\n" +
                $"Number25012012: {Number25012012.ToString()}\r\n" +
                $"CoefficientOfPotentiation: {CoefficientOfPotentiation.ToString()}\r\n" +
                $"PresenceOfTheMaximumPermissibleConcentration: {PresenceOfTheMaximumPermissibleConcentration.ToString()}\r\n" +
                $"MaximumPermissibleConcentrationOneTimemaximum: {MaximumPermissibleConcentrationOneTimemaximum.ToString()}\r\n" +
                $"MaximumPermissibleConcentrationDailyAverage: {MaximumPermissibleConcentrationDailyAverage.ToString()}\r\n" +
                $"ApproximateSafeExposureLevel: {ApproximateSafeExposureLevel.ToString()}\r\n" +
                $"SubstanceHazardClassId: {SubstanceHazardClassId.ToString()}\r\n" +
                $"LimitingIndicatorId: {LimitingIndicatorId.ToString()}\r\n" +
                $"AirContaminants: {(AirContaminants == null ? "" : string.Join(", ", AirContaminants.Select(a => a.ToString())))}";
        }
    }

    public class SummationAirContaminantsGroupIndexPageViewModel
    {
        public IEnumerable<SummationAirContaminantsGroup> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
