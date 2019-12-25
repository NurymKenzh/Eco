using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class KazHydrometWaterPost
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNeedToInput")]
        [Range(0, 9999, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Number")]
        public int Number { get; set; }
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
                $"AdditionalInformationKK: \"{AdditionalInformationKK}\"\r\n" +
                $"AdditionalInformationRU: \"{AdditionalInformationRU}\"\r\n" +
                $"NorthLatitude: {NorthLatitude.ToString()}\r\n" +
                $"EastLongitude: {EastLongitude.ToString()}";
        }

        [NotMapped]
        public KazHydrometWaterPostReport KazHydrometWaterPostReport;
    }

    public class KazHydrometWaterPostIndexPageViewModel
    {
        public IEnumerable<KazHydrometWaterPost> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
