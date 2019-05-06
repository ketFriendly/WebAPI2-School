using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace It_Girls_Projekat.Models.DTOs
{
    public class ClassDto
    {
        [Required]
        [Range(1, 3, ErrorMessage = "The maximum value of Class Number is 3, and the minimum value is 1")]
        public int ClassNo { get; set; }
        [Required]
        public Grades Grade { get; set; }
        public List<StudentDto> Students { get; set; }
       
    }
}