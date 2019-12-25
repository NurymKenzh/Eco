using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class WaterContaminant
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Name")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Number104")]
        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNeedToInput")]
        [Range(0, 9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int? Number104 { get; set; }
        //[Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NumberCAS")]
        //[StringLength(10, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorStringLengthMustBe", MinimumLength = 0)]
        //public string NumberCAS { get; set; }
        //[Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MaximumPermissibleConcentrationWater")]
        //[Range(0, 99.9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        //[DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        //public decimal? MaximumPermissibleConcentrationWater { get; set; }
        //[Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SubstanceHazardClass")]
        //public SubstanceHazardClass SubstanceHazardClass { get; set; }
        //[Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SubstanceHazardClass")]
        //public int SubstanceHazardClassId { get; set; }
        //[Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "LimitingIndicator")]
        //public LimitingIndicator LimitingIndicator { get; set; }
        //[Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "LimitingIndicator")]
        //public int? LimitingIndicatorId { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MeasurementUnit")]
        public MeasurementUnit MeasurementUnit { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MeasurementUnit")]
        public int MeasurementUnitId { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Class1From")]
        [Range(0, 99.9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal? Class1From { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Class1To")]
        [Range(0, 99.9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal? Class1To { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Class2From")]
        [Range(0, 99.9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal? Class2From { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Class2To")]
        [Range(0, 99.9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal? Class2To { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Class3From")]
        [Range(0, 99.9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal? Class3From { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Class3To")]
        [Range(0, 99.9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal? Class3To { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Class4From")]
        [Range(0, 99.9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal? Class4From { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Class4To")]
        [Range(0, 99.9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal? Class4To { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Class5From")]
        [Range(0, 99.9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal? Class5From { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Class5To")]
        [Range(0, 99.9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal? Class5To { get; set; }
        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"Name: {Name}\r\n" +
                $"Number104: {Number104.ToString()}\r\n" +
                $"MeasurementUnitId: {MeasurementUnitId.ToString()}\r\n" +
                $"Class1From: {Class1From.ToString()}\r\n" +
                $"Class1To: {Class1To.ToString()}\r\n" +
                $"Class2From: {Class2From.ToString()}\r\n" +
                $"Class2To: {Class2To.ToString()}\r\n" +
                $"Class3From: {Class3From.ToString()}\r\n" +
                $"Class3To: {Class3To.ToString()}\r\n" +
                $"Class4From: {Class4From.ToString()}\r\n" +
                $"Class4To: {Class4To.ToString()}\r\n" +
                $"Class5From: {Class5From.ToString()}\r\n" +
                $"Class5To: {Class5To.ToString()}";
        }
    }

    public class WaterContaminantIndexPageViewModel
    {
        public IEnumerable<WaterContaminant> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
