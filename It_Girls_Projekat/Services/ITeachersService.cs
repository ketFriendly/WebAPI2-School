
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
    public interface ITeachersService
    {
        IEnumerable<Teacher> Get();
        IEnumerable<TeacherDto> GetAllTeachers();
        TeacherDto GetTeacher(Guid id);
        List<SubjectDto> GetSubjects(Guid id);
        List<Class> GetTeachersClasses(Guid id);
        TeacherDto DeleteTeacher(Guid id);
        TeacherDto ChangeEmail(Guid id, string email);
        string UpdateTeacher(Guid id, string firstName, string lastName, string username, string password, string email);
        TeacherDto GetByUserName(string username);
        UserDto TeacherToUserDto(Teacher teacher);
        Teacher UserDtoToTeacher(UserDto teacher);
        TeacherDto TeachertoTeacherDto(Teacher teacher);
        Teacher TeacherDtoToTeacher(TeacherDto teacherDto);
        
    }
}
