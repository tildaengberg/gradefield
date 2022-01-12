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
using System.Globalization;

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

        public IActionResult Grade()
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


        [HttpPost] //Vafan är select ?
        public IActionResult EditCourse(string status, string betyg, string del)
        {
            
            string s = HttpContext.Session.GetString("session");
            string s2 = HttpContext.Session.GetString("course");

            ViewBag.user = s;

            Methods method = new Methods();
            Course course = method.GetCourse(out string errormsg2, s, Convert.ToInt16(s2));
            if ((string.IsNullOrEmpty(del))){

                if (method.UpdateCourse(out string errormsg, course, s, status, betyg))
                {
                    return RedirectToAction("Courses");
                }
            } else
            {
                if (method.DeleteCourse(out string errormsg3, s, Convert.ToInt16(s2)))
                {
                    return RedirectToAction("Courses");
                }
            }

            ViewBag.del = del;

            return View(course);
        }

        [HttpGet]
        public IActionResult CreateCourse()
        {
            Methods method = new Methods();
            Course course = new Course();

            course.AllInstitutions = method.GetInstitutions(out string errormsg);

            string s = HttpContext.Session.GetString("session");
            ViewBag.user = s;

            return View(course);
        }

        [HttpPost]
        public IActionResult CreateCourse(Course course, string inst)
        {
            
            string s = HttpContext.Session.GetString("session");
            ViewBag.user = s;

            int instID = Convert.ToInt16(inst);
            Methods method = new Methods();

            course.AllInstitutions = method.GetInstitutions(out string errormsg);
            int i = Convert.ToInt16(inst);
            ViewData["inst"] = i;

            if(method.AddCourse(out string errormsg2, course, instID, s))
            {
                return RedirectToAction("Courses");
            } 

           
            return View(course);
        }



        [HttpGet]
        public IActionResult Graduation()
        {
            string s = HttpContext.Session.GetString("session");
            ViewBag.user = s;
            Methods method = new Methods();
            Person person = method.GetExam(out string errmormsg, s);
            person.SumHP = method.GetHP(out string errormsg2, s);


            // År
            ViewData["totYear"] = person.ExamDate.Year - DateTime.Now.Year;

            //Månad
            ViewData["totMonth"] = person.ExamDate.Month - DateTime.Now.Month;

            // Dagar
            if ((person.ExamDate.Day - DateTime.Now.Day) < 0){

                ViewData["totDay"] = DateTime.Now.Day - person.ExamDate.Day;
            }
            else
            {
                ViewData["totDay"] = person.ExamDate.Day - DateTime.Now.Day;
            }

            // Totalt antal dagar kvar
            DateTime thisDay = DateTime.Today;
            TimeSpan tot = person.ExamDate - thisDay;
            ViewData["totalDays"] = tot.Days;

            // Examensdatum
            ViewBag.date = person.ExamDate.ToString("dd MMMM, yyyy", CultureInfo.GetCultureInfo("sv-SE"));
            ViewBag.error = errmormsg;

            // Summa HP
            ViewBag.HP = person.SumHP;

            return View(person);
        }



        [HttpGet]
        public IActionResult Profile()
        {

            Person person = new Person();

            string s = HttpContext.Session.GetString("session");
            ViewBag.user = s;

                

            
            Methods method = new Methods();


            person = method.GetExam(out string errormsg, s);
            person.Education = method.GetEdu(out string errormsg2, s);


            ViewBag.exam = person.ExamDate.ToShortDateString();
            ViewBag.education = person.Education;

            ViewBag.Image = null;
            if (method.GetImg(out string errormsg3, s) != null)
            {

                Byte[] bytes = method.GetImg(out string errormsg4, s);

                ViewBag.Image = ViewImage(bytes);
            }


            return View();
        }

        [HttpPost]
        public IActionResult Profile(Person person)
        {
            string s = HttpContext.Session.GetString("session");

            ViewBag.user = s;

            Methods method = new Methods();
            method.SetEdu(out string errormsg1, s, person.Education);
            method.SetExam(out string errormsg2, s, person.ExamDate);

            ViewBag.error = errormsg2;

            ViewBag.exam = person.ExamDate.ToShortDateString();

            ViewBag.save = "Dina ändringar är sparade";

            ViewBag.Image = null;
            if (method.GetImg(out string errormsg3, s) != null)
            {

                Byte[] bytes = method.GetImg(out string errormsg4, s);

                ViewBag.Image = ViewImage(bytes);
            }


            return View(person);
        }

        [HttpGet]
        public IActionResult EditImage()
        {
            string s = HttpContext.Session.GetString("session");
            ViewBag.user = s;


            return View();
        }

        [HttpPost]
        public IActionResult EditImage(Person person)
        {
            string s = HttpContext.Session.GetString("session");
            ViewBag.user = s;

            // BILDUPPLADDNING
            Methods method = new Methods();

            Byte[] bytes = method.Upload(out string errormsg, person, s);

            
            var stream = new MemoryStream(bytes);
            IFormFile img = new FormFile(stream, 0, bytes.Length, "name", "fileName");
            person.File = img;

            ViewBag.errormsg = errormsg;

            return RedirectToAction("Profile");
        }





        [NonAction]
        private string ViewImage(byte[] arrayImage)
        {
            
            string base64String = Convert.ToBase64String(arrayImage, 0, arrayImage.Length);
            return "data:image/png;base64," + base64String;
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

      
    }
}
