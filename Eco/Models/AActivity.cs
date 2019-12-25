using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class AActivity
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Target")]
        public Target Target { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Target")]
        public int TargetId { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TargetTerritory")]
        public TargetTerritory TargetTerritory { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TargetTerritory")]
        public int TargetTerritoryId { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Event")]
        public Event Event { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Event")]
        public int EventId { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Year")]
        [Range(Constants.YearMin, Constants.YearMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Year { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ActivityType")]
        public bool ActivityType { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ActivityType")]
        public string ActivityTypeName
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    ActivityTypeName = "";
                if (ActivityType)
                {
                    ActivityTypeName = Resources.Controllers.SharedResources.Actual;
                }
                if (!ActivityType)
                {
                    ActivityTypeName = Resources.Controllers.SharedResources.Planned;
                }
                return ActivityTypeName;
            }
        }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ImplementationPercentage")]
        [Range(0, 100, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? ImplementationPercentage { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AdditionalInformationKK")]
        public string AdditionalInformationKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AdditionalInformationRU")]
        public string AdditionalInformationRU { get; set; }
        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"TargetId: {TargetId.ToString()}\r\n" +
                $"TargetTerritoryId: {TargetTerritoryId.ToString()}\r\n" +
                $"EventId: {EventId.ToString()}\r\n" +
                $"Year: {Year.ToString()}\r\n" +
                $"ActivityType: {ActivityType}\r\n" +
                $"ImplementationPercentage: {ImplementationPercentage.ToString()}\r\n" +
                $"AdditionalInformationKK: \"{AdditionalInformationKK}\"\r\n" +
                $"AdditionalInformationRU: \"{AdditionalInformationRU}\"";
        }
    }

    public class AActivityIndexPageViewModel
    {
        public IEnumerable<AActivity> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
