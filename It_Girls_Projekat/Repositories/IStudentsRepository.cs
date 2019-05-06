using It_Girls_Projekat.DTOs;
using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace It_Girls_Projekat.Repositories
{
    public interface IStudentsRepository:IGenericRepository<Student>
    {
        Student ChangeEmail(Guid id, string email);
        bool IsValidEmail(string email);
        Student GetByUserName(string username);
        UserDto StudentToUserDto(Student student);
        Student UserDtoToStudent(UserDto user);
        StudentDto StudentToStudentDto(Student student);
        Student StudentDtoToStudent(StudentDto studentDto);
    }
}
