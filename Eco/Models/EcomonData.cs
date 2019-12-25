using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public enum EcomonType
    {
        AtmosphericPressure = 116,
        PM10 = 117,
        PM25 = 118,
        AirTemperature = 120,
        WindSpeed = 121,
        WindDirection = 122,
        CO = 123 ,
        NO2 = 124,
        SO2 = 125,
        Ozone = 126
    }

    public class EcomonData
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EcomonType")]
        public EcomonType EcomonType { get; set; }
        public long timestamp_ms { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Value")]
        public decimal value { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "DateTime")]
        public DateTime DateTime { get; set; }
        //{
        //    get
        //    {
        //        DateTime DateTime = new DateTime(1970, 1, 1, 6, 0, 0, 0);
        //        DateTime = DateTime.AddSeconds(timestamp_ms / 1000);
        //        return DateTime;
        //    }
        //}
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EcomonNumber")]
        public string EcomonNumber { get; set; }
        //{
        //    get
        //    {
        //        return "11";
        //    }
        //}
    }

    public class EcomonDataIndexPageViewModel
    {
        public IEnumerable<EcomonData> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
