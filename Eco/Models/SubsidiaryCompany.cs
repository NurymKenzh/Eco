using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class SubsidiaryCompany : Company
    {
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "HeadCompany")]
        public Company Company { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "HeadCompany")]
        public int CompanyId { get; set; }
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
                $"CompanyId: {CompanyId.ToString()}";
        }
    }

    public class SubsidiaryCompanyIndexPageViewModel
    {
        public IEnumerable<SubsidiaryCompany> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
