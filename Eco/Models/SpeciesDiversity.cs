using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class SpeciesDiversity
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CityDistrict")]
        public CityDistrict CityDistrict { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CityDistrict")]
        public int CityDistrictId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PlantationsType")]
        public PlantationsType PlantationsType { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PlantationsType")]
        public int PlantationsTypeId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TreesNumber")]
        public int TreesNumber { get; set; }

        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"CityDistrictId: {CityDistrictId.ToString()}\r\n" +
                $"PlantationsTypeId: {PlantationsTypeId.ToString()}\r\n" +
                $"TreesNumber: {TreesNumber.ToString()}";
        }
    }

    public class SpeciesDiversityIndexPageViewModel
    {
        public IEnumerable<SpeciesDiversity> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
