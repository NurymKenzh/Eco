using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class KazHydrometAirPost
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNeedToInput")]
        [Range(0, 9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PostNumber")]
        public int Number { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PostNameKK")]
        public string NameKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PostNameRU")]
        public string NameRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "PostName")]
        public string Name
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    name = NameRU;
                if (language == "kk")
                {
                    name = NameKK;
                }
                if (language == "ru")
                {
                    name = NameRU;
                }
                return name;
            }
        }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Type")]
        public bool Type { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Type")]
        public string TypeName
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    TypeName = "";
                if (Type)
                {
                    TypeName = Resources.Controllers.SharedResources.Ground;
                }
                if (!Type)
                {
                    TypeName = Resources.Controllers.SharedResources.HighAltitude;
                }
                return TypeName;
            }
        }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SamplingType")]
        public bool SamplingType { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SamplingType")]
        public string SamplingTypeName
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    SamplingTypeName = "";
                if (SamplingType)
                {
                    SamplingTypeName = Resources.Controllers.SharedResources.Manual;
                }
                if (!SamplingType)
                {
                    SamplingTypeName = Resources.Controllers.SharedResources.Automated;
                }
                return SamplingTypeName;
            }
        }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SamplingTerm")]
        public SamplingTerm SamplingTerm { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SamplingTerm")]
        public int SamplingTermId { get; set; }
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
                $"Number: {Number.ToString()}\r\n" +
                $"NameKK: {NameKK}\r\n" +
                $"NameRU: {NameRU}\r\n" +
                $"Type: {Type}\r\n" +
                $"SamplingType: {SamplingType}\r\n" +
                $"SamplingTermId: {SamplingTermId.ToString()}\r\n" +
                $"AdditionalInformationKK: \"{AdditionalInformationKK}\"\r\n" +
                $"AdditionalInformationRU: \"{AdditionalInformationRU}\"\r\n" +
                $"NorthLatitude: {NorthLatitude.ToString()}\r\n" +
                $"EastLongitude: {EastLongitude.ToString()}";
        }
        public KazHydrometAirPost()
        {
            Type = true;
            SamplingType = true;
        }

        [NotMapped]
        public KazHydrometAirPostReport KazHydrometAirPostReport;
    }

    public class KazHydrometAirPostIndexPageViewModel
    {
        public IEnumerable<KazHydrometAirPost> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
