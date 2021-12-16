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
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

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
            Methods method = new Methods();
            Person person = new Person
            {
                Failed = method.GetFailed(out string errormsg, s),
                Ongoing = method.GetOngoing(out string errormsg2, s),
                Grades = method.Grades(out string errormsg3,s)

            };

            return View(person);
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
            if (ModelState.IsValid)
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

            else
            {
                return View();
            }
            

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

        public IActionResult Courses()
        {
            string s = HttpContext.Session.GetString("session");
            ViewBag.user = s;
            Methods method = new Methods();
            Person person = new Person
            {
                Courses = method.GetCourses(out string errormsg2, s)

            };

            return View(person);
        }


        [HttpGet]
        public IActionResult EditCourse(string edit)
        {
            int selected = Convert.ToInt16(edit);
            string s = HttpContext.Session.GetString("session");
            ViewBag.user = s;
            Methods method = new Methods();

            Course course = method.GetCourse(out string errormsg, s, selected);
            course.AllStatuses = method.GetStatuses(out string errormsg2);
            course.AllGrades = method.GetGrades(out string errormsg4);


            Status status = method.GetStatus(out string errormsg3, course.Status);
            Grade grade = method.GetGrade(out string errormsg5, course.Betyg);

            HttpContext.Session.SetString("course", edit);

            ViewData["status"] = status.Id;
            ViewData["betyg"] = grade.Id;

            return View(course);
        }


        [HttpPost]
        public IActionResult EditCourse(int select, string status, string betyg)
        {
            
            string s = HttpContext.Session.GetString("session");
            string s2 = HttpContext.Session.GetString("course");

            ViewBag.user = s;

            Methods method = new Methods();
            Course course = method.GetCourse(out string errormsg2, s, Convert.ToInt16(s2));

            if (method.UpdateCourse(out string errormsg, course, s, status, betyg))
            {
                return RedirectToAction("Courses");
            }


            ViewBag.errormsg = errormsg;

            return View(course);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

      
    }
}
