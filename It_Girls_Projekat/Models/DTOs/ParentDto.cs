using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace It_Girls_Projekat.Models.DTOs
{
    public class ParentDto
    {
        public ParentDto()
        {
            Students = new List<StudentDto>();
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

        public List<StudentDto> Students { get; set; }
    }
}