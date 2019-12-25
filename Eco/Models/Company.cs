using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "FullName")]
        public string FullName { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AbbreviatedName")]
        public string AbbreviatedName { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "BIK")]
        public string BIK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "KindOfActivity")]
        public string KindOfActivity { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "HazardClass")]
        public HazardClass HazardClass { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "HazardClass")]
        public int HazardClassId { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "HierarchicalStructure")]
        public bool HierarchicalStructure { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "HierarchicalStructure")]
        public string HierarchicalStructureName
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    HierarchicalStructureName = "";
                if (HierarchicalStructure)
                {
                    HierarchicalStructureName = Resources.Controllers.SharedResources.Yes;
                }
                if (!HierarchicalStructure)
                {
                    HierarchicalStructureName = Resources.Controllers.SharedResources.No;
                }
                return HierarchicalStructureName;
            }
        }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CityDistrict")]
        public CityDistrict CityDistrict { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CityDistrict")]
        public int CityDistrictId { get; set; }
        //[Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Street")]
        //public string LegalAddressStreet { get; set; }
        //[Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "House")]
        //public string LegalAddressHouse { get; set; }
        //[Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Street")]
        //public string ActualAddressStreet { get; set; }
        //[Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "House")]
        //public string ActualAddressHouse { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "LegalAddress")]
        public string LegalAddress { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ActualAddress")]
        public string ActualAddress { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AdditionalInformation")]
        public string AdditionalInformation { get; set; }

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

        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"FullName: {FullName}\r\n" +
                $"AbbreviatedName: {AbbreviatedName}\r\n" +
                $"BIK: {BIK}\r\n" +
                $"KindOfActivity: {KindOfActivity}\r\n" +
                $"HazardClassId: {HazardClassId.ToString()}\r\n" +
                $"HierarchicalStructure: {HierarchicalStructure}\r\n" +
                $"CityDistrictId: {CityDistrictId.ToString()}\r\n" +
                $"LegalAddress: {LegalAddress}\r\n" +
                $"ActualAddress: {ActualAddress}\r\n" +
                $"AdditionalInformation: \"{AdditionalInformation}\"\r\n" +
                $"NorthLatitude: {NorthLatitude.ToString()}\r\n" +
                $"EastLongitude: {EastLongitude.ToString()}";
        }
    }

    public class CompanyIndexPageViewModel
    {
        public IEnumerable<Company> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
