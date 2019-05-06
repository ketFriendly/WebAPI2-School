using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace It_Girls_Projekat.Models.DTOs
{
    public class StudentDto
    {
        public StudentDto()
        {
            new List<ParentDto>();
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
        public List<ParentDto> Parents { get; set; }
        public List<MarkDto> Marks { get; set; }
        public ClassDto Class { get; set; }
    }
}