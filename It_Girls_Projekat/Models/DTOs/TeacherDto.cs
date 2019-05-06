using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace It_Girls_Projekat.Models.DTOs
{
    public class TeacherDto
    {
        public TeacherDto()
        {
            Subjects = new List<SubjectDto>();
        }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public List<SubjectDto>Subjects { get; set; }
    }
}