using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class KazHydrometAirPostData
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "KazHydrometAirPost")]
        public KazHydrometAirPost KazHydrometAirPost { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "KazHydrometAirPost")]
        public int KazHydrometAirPostId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AirContaminant")]
        public AirContaminant AirContaminant { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AirContaminant")]
        public int AirContaminantId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Year")]
        [Range(Constants.YearDataMin, Constants.YearMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Year { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Month")]
        [Range(1, 12, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int? Month { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PollutantConcentrationMonthlyAverage")]
        [DisplayFormat(DataFormatString = Constants.KazHydrometAirPostDataPollutantConcentrationMonthlyAverageDataFormatString, ApplyFormatInEditMode = true)]
        [Range((double)Constants.KazHydrometAirPostDataPollutantConcentrationMonthlyAverageMin, (double)Constants.KazHydrometAirPostDataPollutantConcentrationMonthlyAverageMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? PollutantConcentrationMonthlyAverage { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PollutantConcentrationMaximumOneTimePerMonth")]
        [DisplayFormat(DataFormatString = Constants.KazHydrometAirPostDataPollutantConcentrationMaximumOneTimePerMonthDataFormatString, ApplyFormatInEditMode = true)]
        [Range((double)Constants.KazHydrometAirPostDataPollutantConcentrationMaximumOneTimePerMonthMin, (double)Constants.KazHydrometAirPostDataPollutantConcentrationMaximumOneTimePerMonthMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? PollutantConcentrationMaximumOneTimePerMonth { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PollutantConcentrationMaximumOneTimePerMonthDay")]
        [Range(Constants.DayMin, Constants.DayMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int? PollutantConcentrationMaximumOneTimePerMonthDay { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MaximumPermissibleConcentrationExcessMultiplicityDailyAverage")]
        public decimal? MaximumPermissibleConcentrationExcessMultiplicityDailyAverage
        {
            get
            {
                if(AirContaminant==null)
                {
                    return null;
                }
                return PollutantConcentrationMonthlyAverage / AirContaminant.MaximumPermissibleConcentrationDailyAverage;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MaximumPermissibleConcentrationExcessMultiplicityMaximumOneTime")]
        public decimal? MaximumPermissibleConcentrationExcessMultiplicityMaximumOneTime
        {
            get
            {
                if (AirContaminant == null)
                {
                    return null;
                }
                return PollutantConcentrationMaximumOneTimePerMonth / AirContaminant.MaximumPermissibleConcentrationOneTimemaximum;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PollutantConcentrationMaximumOneTimePerYear")]
        [DisplayFormat(DataFormatString = Constants.KazHydrometAirPostDataPollutantConcentrationMaximumOneTimePerMonthDataFormatString, ApplyFormatInEditMode = true)]
        [Range((double)Constants.KazHydrometAirPostDataPollutantConcentrationMaximumOneTimePerMonthMin, (double)Constants.KazHydrometAirPostDataPollutantConcentrationMaximumOneTimePerMonthMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? PollutantConcentrationMaximumOneTimePerYear { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PollutantConcentrationYearlyAverage")]
        [DisplayFormat(DataFormatString = Constants.KazHydrometAirPostDataPollutantConcentrationMaximumOneTimePerMonthDataFormatString, ApplyFormatInEditMode = true)]
        [Range((double)Constants.KazHydrometAirPostDataPollutantConcentrationMonthlyAverageMin, (double)Constants.KazHydrometAirPostDataPollutantConcentrationMonthlyAverageMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? PollutantConcentrationYearlyAverage { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PollutantConcentrationMaximumOneTimeMonth")]
        [Range(1, 12, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int? PollutantConcentrationMaximumOneTimeMonth { get; set; }

        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"KazHydrometAirPostId: {KazHydrometAirPostId.ToString()}\r\n" +
                $"AirContaminantId: {AirContaminantId.ToString()}\r\n" +
                $"Year: {Year.ToString()}\r\n" +
                $"Month: {Month.ToString()}\r\n" +
                $"PollutantConcentrationMonthlyAverage: {PollutantConcentrationMonthlyAverage.ToString()}\r\n" +
                $"PollutantConcentrationMaximumOneTimePerMonth: {PollutantConcentrationMaximumOneTimePerMonth.ToString()}\r\n" +
                $"PollutantConcentrationMaximumOneTimePerMonthDay: {PollutantConcentrationMaximumOneTimePerMonthDay.ToString()}\r\n" +
                $"PollutantConcentrationMaximumOneTimePerYear: {PollutantConcentrationMaximumOneTimePerYear.ToString()}\r\n" +
                $"PollutantConcentrationYearlyAverage: {PollutantConcentrationYearlyAverage.ToString()}\r\n" +
                $"PollutantConcentrationMaximumOneTimeMonth: {PollutantConcentrationMaximumOneTimeMonth.ToString()}";
        }
    }

    public class KazHydrometAirPostDataIndexPageViewModel
    {
        public IEnumerable<KazHydrometAirPostData> Items { get; set; }
        public Pager Pager { get; set; }
    }

    public class KazHydrometAirContaminantYearReport
    {
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Year")]
        public int Year { get; set; }

        [DisplayFormat(DataFormatString = Constants.KazHydrometAirPostDataPollutantConcentrationMaximumOneTimePerMonthDataFormatString, ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MaximumOneTimeConcentrationPerYearmgm3")]
        public decimal? MaximumOneTimeConcentrationPerYearmgm3 { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MaximumOneTimeConcentrationPerYearDate")]
        [DataType(DataType.Date)]
        public DateTime? MaximumOneTimeConcentrationPerYearDate { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TheMaximumFrequencyOfExceedingTheMaximumPermissibleConcentrationIsMaximumOneTime")]
        public decimal? TheMaximumFrequencyOfExceedingTheMaximumPermissibleConcentrationIsMaximumOneTime { get; set; }
    }

    public struct KazHydrometAirContaminantReport
    {
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AirContaminant")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AverageSeasonalConcentrationWintermgm3")]
        [DisplayFormat(DataFormatString = Constants.KazHydrometAirPostDataPollutantConcentrationMonthlyAverageDataFormatString, ApplyFormatInEditMode = true)]
        public decimal? AverageSeasonalConcentrationWintermgm3 { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AverageSeasonalConcentrationSpringmgm3")]
        [DisplayFormat(DataFormatString = Constants.KazHydrometAirPostDataPollutantConcentrationMonthlyAverageDataFormatString, ApplyFormatInEditMode = true)]
        public decimal? AverageSeasonalConcentrationSpringmgm3 { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AverageSeasonalConcentrationSummermgm3")]
        [DisplayFormat(DataFormatString = Constants.KazHydrometAirPostDataPollutantConcentrationMonthlyAverageDataFormatString, ApplyFormatInEditMode = true)]
        public decimal? AverageSeasonalConcentrationSummermgm3 { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AverageSeasonalConcentrationAutumnmgm3")]
        [DisplayFormat(DataFormatString = Constants.KazHydrometAirPostDataPollutantConcentrationMonthlyAverageDataFormatString, ApplyFormatInEditMode = true)]
        public decimal? AverageSeasonalConcentrationAutumnmgm3 { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AverageAnnualConcentrationmgm3")]
        [DisplayFormat(DataFormatString = Constants.KazHydrometAirPostDataPollutantConcentrationMonthlyAverageDataFormatString, ApplyFormatInEditMode = true)]
        public decimal? AverageAnnualConcentrationmgm3 { get; set; }
        
        public List<KazHydrometAirContaminantYearReport> KazHydrometAirContaminantYearReports { get; set; }
    }

    public struct KazHydrometAirPostReport
    {
        public List<KazHydrometAirContaminantReport> KazHydrometAirContaminantReports { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "StandardIndex")]
        public decimal? StandardIndex { get; set; }
    }
}
