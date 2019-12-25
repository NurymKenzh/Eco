using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class WasteRecyclingCompany
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "BIK")]
        public string BIK { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AddressContactInformation")]
        public string AddressContactInformation { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NorthLatitude")]
        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNeedToInput")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        [Range(0, 99.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal NorthLatitude { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EastLongitude")]
        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNeedToInput")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        [Range(0, 99.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal EastLongitude { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "RecyclableWasteType")]
        public RecyclableWasteType RecyclableWasteType { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "RecyclableWasteType")]
        public int RecyclableWasteTypeId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Status")]
        public bool Status { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Status")]
        public string StatusName
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    StatusName = "";
                if (Status)
                {
                    StatusName = Resources.Controllers.SharedResources.Active;
                }
                if (!Status)
                {
                    StatusName = Resources.Controllers.SharedResources.Inactive;
                }
                return StatusName;
            }
        }

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
                $"Name: {Name}\r\n" +
                $"BIK: {BIK}\r\n" +
                $"AddressContactInformation: \"{AddressContactInformation}\"\r\n" +
                $"NorthLatitude: {NorthLatitude.ToString()}\r\n" +
                $"EastLongitude: {EastLongitude.ToString()}\r\n" +
                $"RecyclableWasteTypeId: {RecyclableWasteTypeId.ToString()}\r\n" +
                $"Status: {Status}\r\n" +
                $"AdditionalInformationKK: \"{AdditionalInformationKK}\"\r\n" +
                $"AdditionalInformationRU: \"{AdditionalInformationRU}\"";
        }
        public WasteRecyclingCompany()
        {
            Status = true;
        }
    }

    public class WasteRecyclingCompanyIndexPageViewModel
    {
        public IEnumerable<WasteRecyclingCompany> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
