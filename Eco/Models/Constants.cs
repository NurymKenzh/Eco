using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class Constants
    {
        public const int YearMin = 2017;
        public const int YearMax = 2025;
        public const int YearDataMin = 2007;
        public const int YearCityDistrictMin = 2000;
        public const int DayMin = 1;
        public const int DayMax = 31;

        public const decimal KazHydrometAirPostDataPollutantConcentrationMonthlyAverageMin = 0;
        public const decimal KazHydrometAirPostDataPollutantConcentrationMonthlyAverageMax = 9.99999M;
        public const string KazHydrometAirPostDataPollutantConcentrationMonthlyAverageDataFormatString = "{0:0.00000}";
        public const decimal KazHydrometAirPostDataPollutantConcentrationMaximumOneTimePerMonthMin = 0;
        public const decimal KazHydrometAirPostDataPollutantConcentrationMaximumOneTimePerMonthMax = 9.99999M;
        public const string KazHydrometAirPostDataPollutantConcentrationMaximumOneTimePerMonthDataFormatString = "{0:0.00000}";

        public const decimal AirPostDataTemperatureCMin = -45;
        public const decimal AirPostDataTemperatureCMax = 45;
        public const string AirPostDataTemperatureCDataFormatString = "{0:0.0}";
        public const decimal AirPostDataAtmosphericPressurekPaMin = 0;
        public const decimal AirPostDataAtmosphericPressurekPaMax = 99.9M;
        public const string AirPostDataAtmosphericPressurekPaDataFormatString = "{0:0.0}";
        public const int AirPostDataHumidityMin = 0;
        public const int AirPostDataHumidityMax = 99;
        public const decimal AirPostDataWindSpeedmsMin = 0;
        public const decimal AirPostDataWindSpeedmsMax = 99.9M;
        public const string AirPostDataWindSpeedmsDataFormatString = "{0:0.0}";
        public const decimal AirPostDataValueMin = 0;
        public const decimal AirPostDataValueMax = 9.99999M;
        public const string AirPostDataValueDataFormatString = "{0:0.00000}";

        public const int TransportPostDataTheLengthOfTheInhibitorySignalSecMin = 0;
        public const int TransportPostDataTheLengthOfTheInhibitorySignalSecMax = 999;
        public const int TransportPostDataTotalNumberOfVehiclesIn20MinutesMin = 0;
        public const int TransportPostDataTotalNumberOfVehiclesIn20MinutesMax = 999;
        public const int TransportPostDataRunningLengthmMin = 0;
        public const int TransportPostDataRunningLengthmMax = 999;
        public const int TransportPostDataAverageSpeedkmhMin = 0;
        public const int TransportPostDataAverageSpeedkmhMax = 99;
        
        public const decimal WaterSurfacePostDataValueMin = 0;
        public const decimal WaterSurfacePostDataValueMax = 9999.99999M;
        public const string WaterSurfacePostDataValueDataFormatString = "{0:0.00000}";

        public const decimal KazHydrometWaterPostDataPollutantConcentrationmglMin = 0;
        public const decimal KazHydrometWaterPostDataPollutantConcentrationmglMax = 99.99999M;
        public const string KazHydrometWaterPostDataPollutantConcentrationmglFormatString = "{0:0.00000}";
    }
}
