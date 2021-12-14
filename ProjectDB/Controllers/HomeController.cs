using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectDB.Models;

namespace ProjectDB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Overview()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {

            Person person = new Person();
            return View();
        }

        [HttpPost]
        public IActionResult Login(Person person)
        {
            Methods method = new Methods();

            if (method.VerifyAccount(out string errormsg, person))
            {
                return RedirectToAction("Overview");
            }


            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
