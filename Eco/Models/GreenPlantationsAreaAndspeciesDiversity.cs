using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class GreenPlantationsAreaAndSpeciesDiversity
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CityDistrict")]
        public CityDistrict CityDistrict { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CityDistrict")]
        public int CityDistrictId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Year")]
        public int Year { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AreaOfGreenCommonAreas")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Range(0, 99999.99, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal AreaOfGreenCommonAreas { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AreaOfGreenPlantationsOfLimitedUse")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Range(0, 99999.99, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal AreaOfGreenPlantationsOfLimitedUse { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AreaOfGreenPlantationsOfSpecialUse")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Range(0, 99999.99, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal AreaOfGreenPlantationsOfSpecialUse { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NumberOfTreeSpecies")]
        [Range(0, 999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int NumberOfTreeSpecies { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AdditionalInformationKK")]
        public string AdditionalInformationKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AdditionalInformationRU")]
        public string AdditionalInformationRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AdditionalInformation")]
        public string AdditionalInformation
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    AdditionalInformation = AdditionalInformationRU;
                if (language == "kk")
                {
                    AdditionalInformation = AdditionalInformationKK;
                }
                if (language == "ru")
                {
                    AdditionalInformation = AdditionalInformationRU;
                }
                return AdditionalInformation;
            }
        }

        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"CityDistrictId: {CityDistrictId.ToString()}\r\n" +
                $"Year: {Year.ToString()}\r\n" +
                $"AreaOfGreenCommonAreas: {AreaOfGreenCommonAreas.ToString()}\r\n" +
                $"AreaOfGreenPlantationsOfLimitedUse: {AreaOfGreenPlantationsOfLimitedUse.ToString()}\r\n" +
                $"AreaOfGreenPlantationsOfSpecialUse: {AreaOfGreenPlantationsOfSpecialUse.ToString()}\r\n" +
                $"NumberOfTreeSpecies: {NumberOfTreeSpecies.ToString()}\r\n" +
                $"AdditionalInformationKK: \"{AdditionalInformationKK}\"\r\n" +
                $"AdditionalInformationRU: \"{AdditionalInformationRU}\"";
        }
    }

    public class GreenPlantationsAreaAndSpeciesDiversityIndexPageViewModel
    {
        public IEnumerable<GreenPlantationsAreaAndSpeciesDiversity> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
