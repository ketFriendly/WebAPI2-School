using It_Girls_Projekat.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace It_Girls_Projekat.Repositories
{
    public interface IAuthRepository : IDisposable
    {
        
        Task<IdentityResult> RegisterAdminUser(Admin userModel, string password);
        Task<IdentityResult> RegisterTeacherUser(Teacher userModel, string password);
        Task<IdentityResult> RegisterParentUser(Parent userModel, string password);
        Task<IdentityResult> RegisterStudentUser(Student userModel, string password);

        Task<ICollection<IdentityResult>> RegisterStudents(IEnumerable<Student> students, string password);

        Task<ApplicationUser> FindUser(string userName, string password);
        Task<IList<string>> FindRoles(string userId);
        ApplicationUser FindUserByUsername(string userName);
    }
}
