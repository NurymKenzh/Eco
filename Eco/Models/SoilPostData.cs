using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class SoilPostData
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SoilPost")]
        public SoilPost SoilPost { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SoilPost")]
        public int SoilPostId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SoilContaminant")]
        public SoilContaminant SoilContaminant { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SoilContaminant")]
        public int SoilContaminantId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "DateOfSampling")]
        [DataType(DataType.Date)]
        public DateTime DateOfSampling { get; set; }
        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "YearOfSampling")]
        [Range(Constants.YearMin, Constants.YearMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int YearOfSampling { get; set; }
        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MonthOfSampling")]
        [Range(1, 12, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int MonthOfSampling { get; set; }
        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "DayOfSampling")]
        [Range(1, 31, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int DayOfSampling { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "GammaBackgroundOfTheSoil")]
        [Range(0, 99.9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal GammaBackgroundOfTheSoil { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ConcentrationValuemgkg")]
        [Range(0, 9999.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        [DisplayFormat(DataFormatString = "{0:0.000000}", ApplyFormatInEditMode = true)]
        public decimal ConcentrationValuemgkg { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MultiplicityOfExcessOfNormative")]
        public decimal? MultiplicityOfExcessOfNormative
        {
            get
            {
                if (SoilContaminant != null)
                {
                    if (SoilContaminant.MaximumPermissibleConcentrationSoil != null)
                    {
                        return ConcentrationValuemgkg / SoilContaminant.MaximumPermissibleConcentrationSoil;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"SoilPostId: {SoilPostId.ToString()}\r\n" +
                $"SoilContaminantId: {SoilContaminantId.ToString()}\r\n" +
                $"DateOfSampling: {DateOfSampling.ToShortDateString()}\r\n" +
                $"GammaBackgroundOfTheSoil: {GammaBackgroundOfTheSoil.ToString()}\r\n" +
                $"ConcentrationValuemgkg: {ConcentrationValuemgkg.ToString()}";
        }
    }

    public class SoilPostDataIndexPageViewModel
    {
        public IEnumerable<SoilPostData> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
