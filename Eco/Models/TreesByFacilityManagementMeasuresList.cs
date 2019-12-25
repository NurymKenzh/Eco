using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class TreesByFacilityManagementMeasuresList
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "GreemPlantsPassport")]
        public GreemPlantsPassport GreemPlantsPassport { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "GreemPlantsPassport")]
        public int GreemPlantsPassportId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PlantationsType")]
        public PlantationsType PlantationsType { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PlantationsType")]
        public int PlantationsTypeId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "BusinessEvents")]
        public PlantationsType BusinessEventsPlantationsType { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "BusinessEvents")]
        public int? BusinessEventsPlantationsTypeId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SanitaryPruning")]
        public int? SanitaryPruning { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CrownFormation")]
        public int? CrownFormation { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SanitaryFelling")]
        public int? SanitaryFelling { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MaintenanceWork")]
        public string MaintenanceWork { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Quantity")]
        public string Quantity { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "BusinessEvents")]
        public bool BusinessEvents { get; set; }

        public TreesByFacilityManagementMeasuresList()
        {
            BusinessEvents = true;
        }

        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"GreemPlantsPassportId: {GreemPlantsPassportId.ToString()}\r\n" +
                $"PlantationsTypeId: {PlantationsTypeId.ToString()}\r\n" +
                $"BusinessEventsPlantationsTypeId: {BusinessEventsPlantationsTypeId.ToString()}\r\n" +
                $"SanitaryPruning: {SanitaryPruning.ToString()}\r\n" +
                $"CrownFormation: {CrownFormation.ToString()}\r\n" +
                $"SanitaryFelling: {SanitaryFelling.ToString()}\r\n" +
                $"MaintenanceWork: {MaintenanceWork}\r\n" +
                $"Quantity: {Quantity}";
        }
    }

    public class TreesByFacilityManagementMeasuresListIndexPageViewModel
    {
        public IEnumerable<TreesByFacilityManagementMeasuresList> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
