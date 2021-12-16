using System;
namespace ProjectDB.Models
{
    public class Grade
    {
        public Grade()
        {
        }

        public int Id { get; set; }
        public string GradeType { get; set; }
        public int Frequency { get; set; }
    }
}
