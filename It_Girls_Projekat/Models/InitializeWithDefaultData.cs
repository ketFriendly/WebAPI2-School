using It_Girls_Projekat.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace It_Girls_Projekat.Models
{
    public class InitializeWithDefaultData : DropCreateDatabaseIfModelChanges<AuthContext>
    {
        protected override void Seed(AuthContext context)
        {
            using (var store = new RoleStore<IdentityRole>(context))
            {
                using (var manager = new RoleManager<IdentityRole>(store))
                {
                    manager.Create(new IdentityRole("Admin"));
                    manager.Create(new IdentityRole("Teacher"));
                    manager.Create(new IdentityRole("Parent"));
                    manager.Create(new IdentityRole("Student"));
                }
            }
            using (var userStore = new UserStore<ApplicationUser>(context))
            {
                using (var userManager = new UserManager<ApplicationUser>(userStore))
                {
                    var salt1 = CryptoHelper.GenerateRandomSalt();
                    ApplicationUser admin = new Admin();
                    admin.Email = "admin@admin.com";
                    admin.UserName = "Sanja";
                    admin.FirstName = "Sanja";
                    admin.LastName = "Mladenovic";
                    
                    userManager.Create(admin, "Web.123");
                    userManager.AddToRole(admin.Id, "Admin");
                }
            }
            //context.SaveChanges();


            //base.Seed(context);
        }

    }
}