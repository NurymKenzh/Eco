using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class WindDirection
    {
        public int Id { get; set; }
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
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Default")]
        public bool Default { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Default")]
        public string DefaultName
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    DefaultName = "";
                if (Default)
                {
                    DefaultName = Resources.Controllers.SharedResources.Default;
                }
                if (!Default)
                {
                    DefaultName = Resources.Controllers.SharedResources.No;
                }
                return DefaultName;
            }
        }
        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"NameKK: {NameKK}\r\n" +
                $"NameRU: {NameRU}\r\n" +
                $"Default: {Default}";
        }
    }

    public class WindDirectionIndexPageViewModel
    {
        public IEnumerable<WindDirection> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
