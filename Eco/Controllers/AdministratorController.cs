using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Eco.Controllers
{
    public class AdministratorController : Controller
    {
        //[Authorize(Roles = "Administrator")]
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [Authorize(Roles = "Administrator")]
        public IActionResult Administration()
        {
            return View();
        }

        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public IActionResult Directories()
        {
            return View();
        }

        [Authorize(Roles = "Administrator, Moderator, Analyst")]
        public IActionResult Targets()
        {
            return View();
        }
    }
}