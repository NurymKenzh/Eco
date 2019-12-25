using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNeedToInput")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorTheFieldIsNotAValidEmailAddress")]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNeedToInput")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorStringLengthMustBe", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "ConfirmNewPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorThePasswordAndConfirmationPasswordDoNotMatch")]
        public string ConfirmPassword { get; set; }
    }
}
