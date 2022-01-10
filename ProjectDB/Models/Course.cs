using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectDB.Models
{
    public class Course
    {
        public Course()
        {
           
        }

        public int ID { get; set; }

        [Display(Name = "Kursnamn*")]
        [Required(ErrorMessage = "Du måste ange ett kursnamn")]
        public string Name { get; set; }

        [Display(Name = "Högskolepoäng*")]
        [Range(0, 300, ErrorMessage = "Värdet måste vara mellan 0 och 300")]
        [Required(ErrorMessage = "Du måste ange antal högskolepoäng")]
        public double HP { get; set; }

        [Display(Name = "Institution*")]
        [Required(ErrorMessage = "Du måste ange en institution")]
        public string Institution { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Betyg")]
        public string Betyg { get; set; }

        public List<Grade> AllGrades = new List<Grade>();

        [Display(Name = "Kursstatus")]
        public List<Status> AllStatuses = new List<Status>();

        [Display(Name = "Institution*")]
        [Required(ErrorMessage = "Du måste ange en institution")]
        public List<Institution> AllInstitutions = new List<Institution>();

    }
}
