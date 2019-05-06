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
    public interface ITeachersRepository:IGenericRepository<Teacher>
    {
        Teacher ChangeEmail(Guid id, string email);
        bool? IsValidEmail(string email);
        //Teacher GetByUserName(string username);
        UserDto TeacherToUserDto(Teacher teacher);
        Teacher UserDtoToTeacher(UserDto user);
        TeacherDto TeachertoTeacherDto(Teacher teacher);
        Teacher TeacherDtoToTeacher(TeacherDto teacherDto);
    }
}
