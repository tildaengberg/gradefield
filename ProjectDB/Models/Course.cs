using System;
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
    }
}
