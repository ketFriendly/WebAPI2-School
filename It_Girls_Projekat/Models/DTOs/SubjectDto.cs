using It_Girls_Projekat.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace It_Girls_Projekat.Models.DTOs
{
    public class SubjectDto
    {
        public int SubjectDtoId { get; set; }
        public SubjectDto()
        {
            Teachers = new List<TeacherDto>();
        }
        public string SubjectName { get; set; }
        public int WeeklyClassNo { get; set; }
        public bool Deleted { get; set; }
        public List<TeacherDto> Teachers { get; set; }

    }
}