using It_Girls_Projekat.DTOs;
using It_Girls_Projekat.Infrastructure;
using It_Girls_Projekat.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace It_Girls_Projekat.Repositories
{
    public class AuthRepository:IDisposable, IAuthRepository
    {

        private UserManager<ApplicationUser> _userManager;


        public AuthRepository(DbContext context)
        {
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        }

       

        public async Task<IdentityResult> RegisterParentUser(Parent userModel, string password)
        {
            var result = await _userManager.CreateAsync(userModel, password);
            _userManager.AddToRole(userModel.Id, "Parent");
            return result;
        }
        
        public async Task<IdentityResult> RegisterAdminUser(Admin userModel, string password)
        {
                var result = await _userManager.CreateAsync(userModel, password);
                _userManager.AddToRole(userModel.Id, "Admin");
                return result;
        }
        
        public async Task<IdentityResult> RegisterTeacherUser(Teacher userModel, string password)
        {
            if (FindUserByUsername(userModel.UserName) == null)
            {
                var result = await _userManager.CreateAsync(userModel, password);
                _userManager.AddToRole(userModel.Id, "Teacher");
                return result;
            }
            return null; 
        }
        public async Task<ICollection<IdentityResult>> RegisterStudents(IEnumerable<Student> students, string password)
        {
            List<IdentityResult> results = new List<IdentityResult>();
            List<Student> users = new List<Student>();

                foreach (var user in students)
                {
                    users.Add(user);
                }
                if (users != null)
                {
                    foreach (var userDto in users)
                    {
                        var result = await RegisterStudentUser(userDto, password);
                        results.Add(result);
                    }
                }
            return results;    
        }
        public async Task<IdentityResult> RegisterStudentUser(Student userModel, string password)
        {
            if (FindUserByUsername(userModel.UserName) == null)
            {
                var result = await _userManager.CreateAsync(userModel, password);
                _userManager.AddToRole(userModel.Id, "Student");
                return result;
            }
            return null;
        }

        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            ApplicationUser user = await _userManager.FindAsync(userName, password);
            return user;
        }
        public ApplicationUser FindUserByUsername(string userName)
        {
            ApplicationUser user = _userManager.FindByName(userName);
            if (user == null)
            {
                return null;
            }
            return user;
        }
        public async Task<IList<string>> FindRoles(string userId)
        {
            return await _userManager.GetRolesAsync(userId);
        }
        public void Dispose()
        {
            if (_userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }
        }
    }
}