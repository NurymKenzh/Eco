using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class SpeciallyProtectedNaturalTerritory
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

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AuthorizedAuthority")]
        public AuthorizedAuthority AuthorizedAuthority { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AuthorizedAuthority")]
        public int AuthorizedAuthorityId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Areahectares")]
        public decimal Areahectares { get; set; }

        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"NameKK: {NameKK}\r\n" +
                $"NameRU: {NameRU}\r\n" +
                $"AuthorizedAuthorityId: {AuthorizedAuthorityId.ToString()}\r\n" +
                $"Areahectares: {Areahectares.ToString()}";
        }
    }

    public class SpeciallyProtectedNaturalTerritoryIndexPageViewModel
    {
        public IEnumerable<SpeciallyProtectedNaturalTerritory> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
