using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace It_Girls_Projekat.Models
{
    public enum Grades { one=1, two, three};
    public class Class
    {
        public Class()
        {
            Students = new List<Student>();
            TeachesSubjects = new List<TeachesSubjects>();
        }
        //[Key]
        public int IdClass { get; set; }
        public int ClassNo { get; set; }
        public Grades Grade { get; set; }
        [JsonIgnore]
        public virtual ICollection<Student> Students { get; set; }
        
        public virtual ICollection<TeachesSubjects> TeachesSubjects { get; set; }

    }
}