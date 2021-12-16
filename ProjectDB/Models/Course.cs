using System;
using System.Collections.Generic;

namespace ProjectDB.Models
{
    public class Course
    {
        public Course()
        {
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public double HP { get; set; }
        public string Institution { get; set; }
        public string Status { get; set; }
        public string Betyg { get; set; }

        public List<Grade> AllGrades = new List<Grade>();
        public List<Status> AllStatuses = new List<Status>();
        public List<Institution> AllInstitutions = new List<Institution>();
    }
}
