using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace It_Girls_Projekat.Models
{
    public class Student : ApplicationUser
    {

        public Student()
        {
            this.Parents = new List<Parent>();
            this.Marks = new List<Mark>();
        }
        
        public int? IdClass { get; set; }
        public virtual Class Class { get; set; }

       // [JsonIgnore]
        public virtual ICollection<Parent> Parents { get; set; }
        [JsonIgnore]
        public virtual ICollection<Mark> Marks { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}