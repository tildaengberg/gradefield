using System;
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
    }
}
