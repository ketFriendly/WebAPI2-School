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
    public interface IStudentsService
    {
        IEnumerable<Student> GetAll();
        IEnumerable<StudentDto> GetAllStudents();
        StudentDto GetStudent(Guid id);
        UserDto DeleteStudent(Guid id);
        StudentDto ChangeEmail(Guid id, string email);
        string UpdateStudent(Guid id, string firstName, string lastName, string username, string password);
        //UserDto GetByUserName(string username);
        string AddExistingParent(Guid id_parent, Guid id_student);
        string AddNewParent(Guid id, UserDto parent, string parent_pass);
        //string AddBulkParentStudent(UserDto user_parent, string pass, UserDto user_student, string stud_pass);
        UserDto StudentToUserDto(Student student);
        Student UserDtoToStudent(UserDto student);
        StudentDto StudentToStudentDto(Student student);
        List<Student> GetByParent(Guid id);
    }
}
