using It_Girls_Projekat.DTOs;
using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace It_Girls_Projekat.Services
{
    public interface IParentsService
    {
        IEnumerable<Parent> Get();
        IEnumerable<UserDto> GetAllParents();
        ParentDto GetParent(Guid id);
        ParentDto DeleteParent(Guid id);
        ParentDto ChangeEmail(Guid id, string email);
        string UpdateParent(Guid id, string firstName, string lastName, string username, string email, string password);
        ParentDto GetByUserName(string username);
        string AddExistingStudent(Guid id_parent, Guid id_student);
        string AddNewStudent(Guid id, UserDto student, string student_pass);
        string AddBulkParentStudent(UserDto user_parent, string pass, UserDto user_student, string stud_pass);
        UserDto ParentToUserDto(Parent parent);
        Parent ParentDtoToParent(ParentDto parentDto);
        ParentDto ParentToParentDto(Parent parent);
    }
}
