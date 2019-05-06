using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace It_Girls_Projekat.Models
{
    public class Subject
    {
        public Subject()
        {
            Teachers = new List<TeachesSubjects>();
        }
        [Key]
        public int IdSubject { get; set; }
        public string Name { get; set; }
        public int WeeklyClassNo { get; set; }
        
        
        [JsonIgnore]
        public virtual ICollection<TeachesSubjects> Teachers { get; set; }
    }
}