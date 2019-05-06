using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace It_Girls_Projekat.Models.DTOs
{
    public class SubjectDtoTeacher
    {
        public int SubjectDtoTeacherId { get; set; }
        public SubjectDtoTeacher()
        {
            Teachers = new List<Teacher>();
        }
        public string SubjectName { get; set; }
        public int WeeklyClassNo { get; set; }
        public bool Deleted { get; set; }
        public List<Teacher> Teachers { get; set; }
    }
}