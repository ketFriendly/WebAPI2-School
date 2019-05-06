using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace It_Girls_Projekat.Models
{
    public class TeachesSubjects
    {
        public TeachesSubjects()
        {
            Marks = new List<Mark>();
            Classes = new List<Class>();
        }
        [Key]
        public int IdTeachesSubject { get; set; }

        public Guid IdTeacher { get; set; }
        public virtual Teacher Teacher { get; set; }
        public int IdSubject { get; set; }
        public virtual Subject Subject { get; set; }
        [JsonIgnore]
        public virtual ICollection<Mark> Marks { get; set; }
        [JsonIgnore]
        public virtual ICollection<Class> Classes { get; set; }
    }
}