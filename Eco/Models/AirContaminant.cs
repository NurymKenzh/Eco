using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class AirContaminant
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Name")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Number168")]
        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNeedToInput")]
        [Range(0, 9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int? Number168 { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Number104")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNeedToInput")]
        [Range(0, 9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int? Number104 { get; set; }
        //[Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ContaminantCodeERA")]
        //[StringLength(4, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorStringLengthMustBe", MinimumLength = 1)]
        //public string ContaminantCodeERA { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NumberCAS")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorStringLengthMustBe", MinimumLength = 0)]
        public string NumberCAS { get; set; }
        //[Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Synonyms")]
        //public string[] Synonyms { get; set; }
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
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CoefficientOfSettlement")]
        [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
        [Range(0, 9.9, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal CoefficientOfSettlement { get; set; }
        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"Name: {Name}\r\n" +
                $"Number168: {Number168.ToString()}\r\n" +
                $"Number104: {Number104.ToString()}\r\n" +
                //$"ContaminantCodeERA: {ContaminantCodeERA}\r\n" +
                $"NumberCAS: {NumberCAS}\r\n" +
                //$"Synonyms: {string.Join(", ", Synonyms)}\r\n" +
                $"PresenceOfTheMaximumPermissibleConcentration: {PresenceOfTheMaximumPermissibleConcentration.ToString()}\r\n" +
                $"MaximumPermissibleConcentrationOneTimemaximum: {MaximumPermissibleConcentrationOneTimemaximum.ToString()}\r\n" +
                $"MaximumPermissibleConcentrationDailyAverage: {MaximumPermissibleConcentrationDailyAverage.ToString()}\r\n" +
                $"ApproximateSafeExposureLevel: {ApproximateSafeExposureLevel.ToString()}\r\n" +
                $"SubstanceHazardClassId: {SubstanceHazardClassId.ToString()}\r\n" +
                $"LimitingIndicatorId: {LimitingIndicatorId.ToString()}";
        }
    }

    public class AirContaminantIndexPageViewModel
    {
        public IEnumerable<AirContaminant> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
