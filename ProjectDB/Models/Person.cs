using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectDB.Models
{
    public class Person
    {
        public Person() {}

        [Required]
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime ExamDate { get; set; }
        public string Education { get; set; }


        // Alla kurser
        public List<Course> Courses = new List<Course>();

        // Failade kurser
        public List<Course> Failed = new List<Course>();

        // Pågående kurser
        public List<Course> Ongoing = new List<Course>();
    }
}
