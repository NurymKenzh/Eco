using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class AirPollutionIndicator
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TypeOfAirPollutionIndicator")]
        public TypeOfAirPollutionIndicator TypeOfAirPollutionIndicator { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TypeOfAirPollutionIndicator")]
        public int TypeOfAirPollutionIndicatorId { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Name")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Description")]
        public string Description { get; set; }
        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"TypeOfAirPollutionIndicatorId: {TypeOfAirPollutionIndicatorId.ToString()}\r\n" +
                $"Name: {Name}\r\n" +
                $"Description: \"{Description}\"";
        }
    }

    public class AirPollutionIndicatorIndexPageViewModel
    {
        public IEnumerable<AirPollutionIndicator> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
