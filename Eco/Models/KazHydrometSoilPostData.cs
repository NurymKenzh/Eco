using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class KazHydrometSoilPostData
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "KazHydrometSoilPost")]
        public KazHydrometSoilPost KazHydrometSoilPost { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "KazHydrometSoilPost")]
        public int KazHydrometSoilPostId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SoilContaminant")]
        public SoilContaminant SoilContaminant { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SoilContaminant")]
        public int SoilContaminantId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Year")]
        [Range(Constants.YearDataMin, Constants.YearMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Year { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Season")]
        public bool Season { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Season")]
        public string SeasonName
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    SeasonName = "";
                if (Season)
                {
                    SeasonName = Resources.Controllers.SharedResources.Spring;
                }
                if (!Season)
                {
                    SeasonName = Resources.Controllers.SharedResources.Autumn;
                }
                return SeasonName;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ConcentrationValuemgkg")]
        [Range(0, 9999.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        [DisplayFormat(DataFormatString = "{0:0.000000}", ApplyFormatInEditMode = true)]
        public decimal ConcentrationValuemgkg { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MultiplicityOfExcessOfTheMaximumPermissibleConcentration")]
        public decimal? MultiplicityOfExcessOfTheMaximumPermissibleConcentration
        {
            get
            {
                if(SoilContaminant!=null)
                {
                    if(SoilContaminant.MaximumPermissibleConcentrationSoil !=null)
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
                $"KazHydrometSoilPostId: {KazHydrometSoilPostId.ToString()}\r\n" +
                $"SoilContaminantId: {SoilContaminantId.ToString()}\r\n" +
                $"Year: {Year.ToString()}\r\n" +
                $"Season: {Season.ToString()}\r\n" +
                $"ConcentrationValuemgkg: {ConcentrationValuemgkg.ToString()}";
        }

        public KazHydrometSoilPostData()
        {
            Season = true;
            Year = DateTime.Today.Year;
        }
    }

    public class KazHydrometSoilPostDataIndexPageViewModel
    {
        public IEnumerable<KazHydrometSoilPostData> Items { get; set; }
        public Pager Pager { get; set; }
    }

    public struct SoilContaminantReport
    {
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SoilContaminant")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TheMaximumFrequencyOfExceedingTheMaximumPermissibleConcentration")]
        public decimal? TheMaximumFrequencyOfExceedingTheMaximumPermissibleConcentration { get; set; }
    }

    public struct KazHydrometSoilPostReport
    {
        public List<SoilContaminantReport> SoilContaminantReports { get; set; }
    }
}
