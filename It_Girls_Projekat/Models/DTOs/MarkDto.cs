using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace It_Girls_Projekat.Models.DTOs
{
    
    public class MarkDto
    {
        public string StudentsName { get; set; }
        public string TeachersName { get; set; }
        public string Mark { get; set; }
        public string Subject { get; set; }
        [JsonIgnore]
        public EAccessType AccessType { get; set; }
        public bool ShouldSerializeStudentsName()
        {
            return AccessType != EAccessType.Student;
        }
        public bool ShouldSerializeTeachersName()
        {
            return AccessType != EAccessType.Teacher;
        }
        public bool ShouldSerializeMark()
        {
            return AccessType != EAccessType.Public;
        }
        public bool ShouldSerializeSubject()
        {
            return AccessType != EAccessType.Public;
        }
    }
}