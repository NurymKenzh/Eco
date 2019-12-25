using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class KazHydrometWaterPostData
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "KazHydrometWaterPost")]
        public KazHydrometWaterPost KazHydrometWaterPost { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "KazHydrometWaterPost")]
        public int KazHydrometWaterPostId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WaterContaminant")]
        public WaterContaminant WaterContaminant { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WaterContaminant")]
        public int WaterContaminantId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Year")]
        [Range(Constants.YearMin, Constants.YearMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Year { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Month")]
        [Range(1, 12, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Month { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PollutantConcentrationmgl")]
        [DisplayFormat(DataFormatString = Constants.KazHydrometWaterPostDataPollutantConcentrationmglFormatString, ApplyFormatInEditMode = true)]
        [Range((double)Constants.KazHydrometWaterPostDataPollutantConcentrationmglMin, (double)Constants.KazHydrometWaterPostDataPollutantConcentrationmglMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal PollutantConcentrationmgl { get; set; }

        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"KazHydrometWaterPostId: {KazHydrometWaterPostId.ToString()}\r\n" +
                $"WaterContaminantId: {WaterContaminantId.ToString()}\r\n" +
                $"Year: {Year.ToString()}\r\n" +
                $"Month: {Month.ToString()}\r\n" +
                $"PollutantConcentrationmgl: {PollutantConcentrationmgl.ToString()}";
        }
    }

    public class KazHydrometWaterPostDataIndexPageViewModel
    {
        public IEnumerable<KazHydrometWaterPostData> Items { get; set; }
        public Pager Pager { get; set; }
    }

    public struct KazHydrometWaterContaminantReport
    {
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WaterContaminant")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AverageSeasonalConcentrationWintermgl")]
        [DisplayFormat(DataFormatString = Constants.KazHydrometWaterPostDataPollutantConcentrationmglFormatString, ApplyFormatInEditMode = true)]
        public decimal? AverageSeasonalConcentrationWintermgl { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AverageSeasonalConcentrationSpringmgl")]
        [DisplayFormat(DataFormatString = Constants.KazHydrometWaterPostDataPollutantConcentrationmglFormatString, ApplyFormatInEditMode = true)]
        public decimal? AverageSeasonalConcentrationSpringmgl { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AverageSeasonalConcentrationSummermgl")]
        [DisplayFormat(DataFormatString = Constants.KazHydrometWaterPostDataPollutantConcentrationmglFormatString, ApplyFormatInEditMode = true)]
        public decimal? AverageSeasonalConcentrationSummermgl { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AverageSeasonalConcentrationAutumnmgl")]
        [DisplayFormat(DataFormatString = Constants.KazHydrometWaterPostDataPollutantConcentrationmglFormatString, ApplyFormatInEditMode = true)]
        public decimal? AverageSeasonalConcentrationAutumnmgl { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AverageAnnualConcentrationmgl")]
        [DisplayFormat(DataFormatString = Constants.KazHydrometWaterPostDataPollutantConcentrationmglFormatString, ApplyFormatInEditMode = true)]
        public decimal? AverageAnnualConcentrationmgl { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MaximumMonthlyConcentrationmgl")]
        [DisplayFormat(DataFormatString = Constants.KazHydrometWaterPostDataPollutantConcentrationmglFormatString, ApplyFormatInEditMode = true)]
        public decimal?[] MaxConcentrationMonths { get; set; }
    }

    public struct KazHydrometWaterPostReport
    {
        public List<KazHydrometWaterContaminantReport> KazHydrometWaterContaminantReports { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "StandardIndex")]
        public decimal? StandardIndex { get; set; }
    }
}
