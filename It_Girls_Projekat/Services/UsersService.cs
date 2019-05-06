using It_Girls_Projekat.DTOs;
using It_Girls_Projekat.Models;
using It_Girls_Projekat.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace It_Girls_Projekat.Services
{
    public class UsersService : IUsersService
    {
        private IUnitOfWork db;

        public UsersService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }
        //public IEnumerable<ApplicationUser>GetAllUsers()
        //{
        //   IEnumerable<ApplicationUser> users = db.AuthRepository.Get();
        //    return users;
        //}

        public async Task<IdentityResult> RegisterTeacher(UserDto teacher)
        {
            Teacher user = new Teacher
            {
                UserName = teacher.UserName,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
            };
            Task<IdentityResult> task = db.AuthRepository.RegisterTeacherUser(user, teacher.Password);
            if (task == null)
            {
                return null;
            }
            return await task;
        }

        public async Task<IdentityResult> RegisterAdmin(UserDto admin)
        {
            Admin user = new Admin
            {
                UserName = admin.UserName,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                ShortName = "tx"
            };
            return await db.AuthRepository.RegisterAdminUser(user, admin.Password);
        }



        public async Task<IdentityResult> RegisterStudent(UserDto student)
        {
            Student user = new Student
            {
                UserName = student.UserName,
                FirstName = student.FirstName,
                LastName = student.LastName
            };
            Task<IdentityResult> task = db.AuthRepository.RegisterStudentUser(user, student.Password);
            if (task == null)
            {
                return null;
            }
            return await task;
        }

        public async Task<IdentityResult> RegisterParent(UserDto parent)
        {
            Parent user = new Parent
            {
                UserName = parent.UserName,
                FirstName = parent.FirstName,
                LastName = parent.LastName,
                Email = parent.Email
            };
            return await db.AuthRepository.RegisterParentUser(user, parent.Password);
        }
        public async Task<ICollection<IdentityResult>> RegisterStudents(IEnumerable<UserDto> students)
        {
            List<Student> bulkAddStudents = new List<Student>();
            List<IdentityResult> results = new List<IdentityResult>();
            foreach (var student in students)
            {
                Student user = new Student
                {
                    UserName = student.UserName,
                    FirstName = student.FirstName,
                    LastName = student.LastName
                };
                var result = await db.AuthRepository.RegisterStudentUser(user, student.Password);
                results.Add(result);
            }
            return results;
        }

    }
}