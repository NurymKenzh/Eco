using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNeedToInput")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorTheFieldIsNotAValidEmailAddress")]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNeedToInput")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorStringLengthMustBe", MinimumLength = 5)]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}
