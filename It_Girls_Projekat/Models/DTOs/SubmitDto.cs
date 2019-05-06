using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace It_Girls_Projekat.Models.DTOs
{
    public class SubmitDto
    {
        public string teacherId { get; set; }
        public string studentId { get; set; }
        public int subjectId { get; set; }
        public int mark { get; set; }
    }
}