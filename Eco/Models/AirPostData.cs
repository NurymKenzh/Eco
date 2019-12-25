using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class AirPostData
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AirPost")]
        public AirPost AirPost { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AirPost")]
        public int AirPostId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AirContaminant")]
        public AirContaminant AirContaminant { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AirContaminant")]
        public int AirContaminantId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "DateTime")]
        public DateTime DateTime { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Year")]
        [Range(Constants.YearDataMin, Constants.YearMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Year { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Month")]
        [Range(1, 12, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Month { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Day")]
        [Range(1, 31, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Day { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Hour")]
        [Range(0, 23, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Hour { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Minute")]
        [Range(0, 59, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Minute { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Second")]
        [Range(0, 59, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Second { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TemperatureC")]
        [DisplayFormat(DataFormatString = Constants.AirPostDataTemperatureCDataFormatString, ApplyFormatInEditMode = true)]
        [Range((double)Constants.AirPostDataTemperatureCMin, (double)Constants.AirPostDataTemperatureCMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal TemperatureC { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AtmosphericPressurekPa")]
        [DisplayFormat(DataFormatString = Constants.AirPostDataAtmosphericPressurekPaDataFormatString, ApplyFormatInEditMode = true)]
        [Range((double)Constants.AirPostDataAtmosphericPressurekPaMin, (double)Constants.AirPostDataAtmosphericPressurekPaMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal AtmosphericPressurekPa { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Humidity")]
        [Range(Constants.AirPostDataHumidityMin, Constants.AirPostDataHumidityMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Humidity { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WindSpeedms")]
        [DisplayFormat(DataFormatString = Constants.AirPostDataWindSpeedmsDataFormatString, ApplyFormatInEditMode = true)]
        [Range((double)Constants.AirPostDataWindSpeedmsMin, (double)Constants.AirPostDataWindSpeedmsMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal WindSpeedms { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WindDirection")]
        public WindDirection WindDirection { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WindDirection")]
        public int WindDirectionId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "GeneralWeatherCondition")]
        public GeneralWeatherCondition GeneralWeatherCondition { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "GeneralWeatherCondition")]
        public int GeneralWeatherConditionId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Value")]
        [DisplayFormat(DataFormatString = Constants.AirPostDataValueDataFormatString, ApplyFormatInEditMode = true)]
        [Range((double)Constants.AirPostDataValueMin, (double)Constants.AirPostDataValueMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal Value { get; set; }

        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"AirPostId: {AirPostId.ToString()}\r\n" +
                $"AirContaminantId: {AirContaminantId.ToString()}\r\n" +
                $"DateTime: {DateTime.ToString()}\r\n" +
                $"TemperatureC: {TemperatureC.ToString()}\r\n" +
                $"AtmosphericPressurekPa: {AtmosphericPressurekPa.ToString()}\r\n" +
                $"Humidity: {Humidity.ToString()}\r\n" +
                $"WindSpeedms: {WindSpeedms.ToString()}\r\n" +
                $"WindDirectionId: {WindDirectionId.ToString()}\r\n" +
                $"GeneralWeatherConditionId: {GeneralWeatherConditionId.ToString()}\r\n" +
                $"Value: {Value.ToString()}";
        }
    }

    public class AirPostDataIndexPageViewModel
    {
        public IEnumerable<AirPostData> Items { get; set; }
        public Pager Pager { get; set; }
    }

    public struct AirContaminantReport
    {
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AirContaminant")]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = Constants.KazHydrometAirPostDataPollutantConcentrationMaximumOneTimePerMonthDataFormatString, ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MaximumOneTimeValue")]
        public decimal? MaximumOneTimeValue { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MaximumOneTimeValueDate")]
        [DataType(DataType.Date)]
        public DateTime? MaximumOneTimeValueDate { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TheMaximumFrequencyOfExceedingTheMaximumPermissibleConcentrationIsMaximumOneTime")]
        public decimal? TheMaximumFrequencyOfExceedingTheMaximumPermissibleConcentrationIsMaximumOneTime { get; set; }
    }

    public struct AirPostReport
    {
        public List<AirContaminantReport> AirContaminantReports { get; set; }
    }
}
