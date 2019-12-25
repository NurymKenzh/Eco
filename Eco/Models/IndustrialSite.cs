using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class IndustrialSite
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Company")]
        public Company Company { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Company")]
        public int CompanyId { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SubsidiaryCompany")]
        public SubsidiaryCompany SubsidiaryCompany { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SubsidiaryCompany")]
        public int? SubsidiaryCompanyId { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Company")]
        public string CompanyOrSubsidiaryCompanyAbbreviatedName
        {
            get
            {
                string CompanyOrSubsidiaryCompanyAbbreviatedName = Company != null ? $"{Company.AbbreviatedName}" : "";
                CompanyOrSubsidiaryCompanyAbbreviatedName += 
                    CompanyOrSubsidiaryCompanyAbbreviatedName != "" ?
                    (SubsidiaryCompany != null ? $" ({SubsidiaryCompany.AbbreviatedName})" : "") :
                    (SubsidiaryCompany != null ? $"{SubsidiaryCompany.AbbreviatedName}" : "");
                return CompanyOrSubsidiaryCompanyAbbreviatedName;
            }
        }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "FullName")]
        public string FullName { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AbbreviatedName")]
        public string AbbreviatedName { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "HazardClass")]
        public HazardClass HazardClass { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "HazardClass")]
        public int HazardClassId { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CityDistrict")]
        public CityDistrict CityDistrict { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CityDistrict")]
        public int CityDistrictId { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Street")]
        public string Street { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "House")]
        public string House { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NorthLatitude")]
        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNeedToInput")]
        [DisplayFormat(DataFormatString = "{0:0.G}", ApplyFormatInEditMode = true)]
        [Range(0, 99.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal NorthLatitude { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EastLongitude")]
        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNeedToInput")]
        [DisplayFormat(DataFormatString = "{0:0.G}", ApplyFormatInEditMode = true)]
        [Range(0, 99.999999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal EastLongitude { get; set; }
        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"CompanyId: {CompanyId.ToString()}\r\n" +
                $"SubsidiaryCompanyId: {SubsidiaryCompanyId.ToString()}\r\n" +
                $"FullName: {FullName}\r\n" +
                $"AbbreviatedName: {AbbreviatedName}\r\n" +
                $"HazardClassId: {HazardClassId.ToString()}\r\n" +
                $"CityDistrictId: {CityDistrictId.ToString()}\r\n" +
                $"Street: {Street}\r\n" +
                $"House: {House}\r\n" +
                $"NorthLatitude: {NorthLatitude.ToString()}\r\n" +
                $"EastLongitude: {EastLongitude.ToString()}";
        }
    }

    public class IndustrialSiteIndexPageViewModel
    {
        public IEnumerable<IndustrialSite> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
