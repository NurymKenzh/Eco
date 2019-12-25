using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class Log
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Email")]
        public string Email { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "DateTime")]
        public DateTime DateTime { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Class")]
        public string Class { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Operation")]
        public string Operation { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "OldValues")]
        public string Old { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NewValues")]
        public string New { get; set; }
    }

    public class LogIndexPageViewModel
    {
        public IEnumerable<Log> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
