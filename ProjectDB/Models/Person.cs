using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectDB.Models
{
    public class Person
    {
        public Person() {}

        [Display(Name = "Användarnamn*")]
        [Required(ErrorMessage = "Du måste fylla i användarnamn")]
        public string Username { get; set; }

        [Display(Name = "Lösenord*")]
        [ScaffoldColumn(true)]
        [MinLength(5, ErrorMessage = "Lösenord måste bestå av minst 5 tecken")]
        [Required(ErrorMessage = "Du måste fylla i lösenord")]
        public string Password { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Examensdatum*")]
        [Required(ErrorMessage = "Du måste ange ett examensdatum")]
        public DateTime ExamDate { get; set; }

        [Display(Name = "Utbildning*")]
        [Required(ErrorMessage = "Du måste ange en utbildning")]
        public string Education { get; set; }

        public double SumHP { get; set; }


        // Alla kurser
        public List<Course> Courses = new List<Course>();

        // Failade kurser
        public List<Course> Failed = new List<Course>();

        // Pågående kurser
        public List<Course> Ongoing = new List<Course>();

        public List<Grade> Grades = new List<Grade>();
    }
}
