using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class PlantationsState
    {
        public int Id { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CityDistrict")]
        public CityDistrict CityDistrict { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CityDistrict")]
        public int CityDistrictId { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PlantationsStateType")]
        public PlantationsStateType PlantationsStateType { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PlantationsStateType")]
        public int PlantationsStateTypeId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TreesNumber")]
        public decimal TreesNumber { get; set; }

        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"CityDistrictId: {CityDistrictId.ToString()}\r\n" +
                $"PlantationsStateTypeId: {PlantationsStateTypeId.ToString()}\r\n" +
                $"TreesNumber: {TreesNumber.ToString()}";
        }
    }

    public class PlantationsStateIndexPageViewModel
    {
        public IEnumerable<PlantationsState> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
