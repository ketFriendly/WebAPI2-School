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
    public class Teacher : ApplicationUser
    {
        public Teacher()
        {
            Subjects = new List<TeachesSubjects>();
        }
        [JsonIgnore]
        public virtual ICollection<TeachesSubjects> Subjects { get; set; }

        public  async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}