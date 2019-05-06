using It_Girls_Projekat.DTOs;
using It_Girls_Projekat.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace It_Girls_Projekat.Services
{
    public interface IUsersService
    {
        //IEnumerable<ApplicationUser> GetAllUsers();
        Task<IdentityResult> RegisterTeacher(UserDto teacher);
        Task<IdentityResult> RegisterAdmin(UserDto admin);
        Task<IdentityResult> RegisterStudent(UserDto student);
        Task<IdentityResult> RegisterParent(UserDto parent);

        Task<ICollection<IdentityResult>> RegisterStudents(IEnumerable<UserDto> students);
    }
}
