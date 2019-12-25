using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class AuthorizedAuthority
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Name")]
        public string Name { get; set; }
        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"Name: {Name}";
        }
    }

    public class AuthorizedAuthorityIndexPageViewModel
    {
        public IEnumerable<AuthorizedAuthority> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
