using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectDB.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

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
            string s = HttpContext.Session.GetString("session");
            ViewBag.user = s;
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
                string s = person.Username;
                HttpContext.Session.SetString("session", s);
                return RedirectToAction("Overview");
            }

            ViewBag.errormsg = errormsg;

            

            return View();
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(Person person)
        {

            Methods method = new Methods();

            if (method.CreateAccount(out string errormsg, person))
            {
                string s = person.Username;
                HttpContext.Session.SetString("session", s);
                return RedirectToAction("Overview");
            }

            ViewBag.errormsg = errormsg;



            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
