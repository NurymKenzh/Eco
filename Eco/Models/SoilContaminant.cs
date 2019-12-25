using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class SoilContaminant
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NormativeValue")]
        [Range(0, 9999.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        [DisplayFormat(DataFormatString = "{0:0.000000}", ApplyFormatInEditMode = true)]
        public decimal? MaximumPermissibleConcentrationSoil { get; set; }

        //[Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "LimitingSoilIndicator")]
        //public LimitingSoilIndicator LimitingSoilIndicator { get; set; }
        //[Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "LimitingSoilIndicator")]
        //public int? LimitingSoilIndicatorId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NormativeSoilType")]
        public NormativeSoilType NormativeSoilType { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NormativeSoilType")]
        public int NormativeSoilTypeId { get; set; }


        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"Name: {Name}\r\n" +
                $"MaximumPermissibleConcentrationSoil: {MaximumPermissibleConcentrationSoil.ToString()}\r\n" +
                $"NormativeSoilTypeId: {NormativeSoilTypeId.ToString()}";
        }
    }

    public class SoilContaminantIndexPageViewModel
    {
        public IEnumerable<SoilContaminant> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
