using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace It_Girls_Projekat.Models
{
    public enum Marks { F=1, D, C, B, A}
    public class Mark
    {
        [Key]
        public int IdMark { get; set; }
        public Marks Marks { get; set; }
        public int? IdTeachesSubject { get; set; }
        public virtual TeachesSubjects TeacherAndSubject { get; set; }
        public string IdStudent { get; set; } 
        public virtual Student Student { get; set; }
        
    }
}