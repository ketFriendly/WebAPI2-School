using It_Girls_Projekat.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace It_Girls_Projekat.Infrastructure
{
    public class AuthContext:IdentityDbContext<ApplicationUser>
    {
        public AuthContext():base("AuthContext")
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AuthContext>());
            Database.SetInitializer(new InitializeWithDefaultData());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Parent>().ToTable("Parent");
            modelBuilder.Entity<Admin>().ToTable("Admin");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Teacher>().ToTable("Teacher");
            modelBuilder.Entity<TeachesSubjects>().ToTable("TeachesSubjects");
             //modelBuilder.Entity<IdentityRole>().ToTable("Role");
                //.HasKey<int>(s => s.IdTeachesSubject);
                
            modelBuilder.Entity<Mark>().ToTable("Mark")/*.HasKey<int>(s => s.IdMark)*/;
            modelBuilder.Entity<Class>().ToTable("Class").HasKey<int>(s => s.IdClass);
            modelBuilder.Entity<Subject>().ToTable("Subject")/*.HasKey<int>(s => s.IdSubject)*/;

            //modelBuilder.Entity<Teacher>()
            //    .HasMany<TeachesSubjects>(s => s.Subjects);
            //modelBuilder.Entity<Subject>()
            //    .HasMany<>


            //modelBuilder.Entity<Parent>()
            //    .HasMany<Student>(s => s.Students)
            //    .WithMany(p => p.Parents)
            // .Map(sp =>
            //{
            //    sp.MapLeftKey("ParentId");
            //    sp.MapRightKey("StudentId");
            //    sp.ToTable("StudentParents");
            //});
            //modelBuilder.Entity<Student>()
            //    .HasMany<Parent>(s => s.Parents)
            //    .WithMany(p => p.Students);
            //    //.Map(sp =>
            //    //{
            //    //    sp.MapLeftKey("ParentId");
            //    //    sp.MapRightKey("StudentId");
            //    //    sp.ToTable("StudentParents");
            //    //});
        }

    }
}