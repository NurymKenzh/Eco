using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class GreenPlantationsState
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CityDistrict")]
        public CityDistrict CityDistrict { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CityDistrict")]
        public int CityDistrictId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NameKK")]
        public string NameKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NameRU")]
        public string NameRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Name")]
        public string Name
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    Name = NameRU;
                if (language == "kk")
                {
                    Name = NameKK;
                }
                if (language == "ru")
                {
                    Name = NameRU;
                }
                return Name;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "GreenPlantationsType")]
        public GreenPlantationsType GreenPlantationsType { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "GreenPlantationsType")]
        public int GreenPlantationsTypeId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Areahectares")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Range(0, 999999.99, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal Areahectares { get; set; }

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
                $"NameKK: {NameKK}\r\n" +
                $"NameRU: {NameRU}\r\n" +
                $"GreenPlantationsTypeId: {GreenPlantationsTypeId.ToString()}\r\n" +
                $"Areahectares: {Areahectares.ToString()}\r\n" +
                $"AdditionalInformationKK: \"{AdditionalInformationKK}\"\r\n" +
                $"AdditionalInformationRU: \"{AdditionalInformationRU}\"";
        }
    }

    public class GreenPlantationsStateIndexPageViewModel
    {
        public IEnumerable<GreenPlantationsState> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
