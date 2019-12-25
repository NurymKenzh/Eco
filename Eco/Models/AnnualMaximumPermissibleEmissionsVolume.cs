using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class AnnualMaximumPermissibleEmissionsVolume
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

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "IssuingPermitsStateAuthority")]
        public IssuingPermitsStateAuthority IssuingPermitsStateAuthority { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "IssuingPermitsStateAuthority")]
        public int IssuingPermitsStateAuthorityId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Year")]
        [Range(Constants.YearDataMin, Constants.YearMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int? YearOfPermit { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "DateOfIssueOfPermit")]
        [DataType(DataType.Date)]
        public DateTime DateOfIssueOfPermit { get; set; }
        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Year")]
        [Range(Constants.YearDataMin, Constants.YearMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Year{ get; set; }
        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Month")]
        [Range(1, 12, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Month{ get; set; }
        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Day")]
        [Range(1, 31, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Day{ get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "EmissionsTonsPerYear")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        [Range(0, 999999.9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal EmissionsTonsPerYear { get; set; }

        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"CompanyId: {CompanyId.ToString()}\r\n" +
                $"SubsidiaryCompanyId: {SubsidiaryCompanyId.ToString()}\r\n" +
                $"IssuingPermitsStateAuthorityId: {IssuingPermitsStateAuthorityId.ToString()}\r\n" +
                $"YearOfPermit: {YearOfPermit.ToString()}\r\n" +
                $"DateOfIssueOfPermit: {DateOfIssueOfPermit.ToShortDateString()}\r\n" +
                $"EmissionsTonsPerYear: {EmissionsTonsPerYear.ToString()}";
        }
    }

    public class AnnualMaximumPermissibleEmissionsVolumeIndexPageViewModel
    {
        public IEnumerable<AnnualMaximumPermissibleEmissionsVolume> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
