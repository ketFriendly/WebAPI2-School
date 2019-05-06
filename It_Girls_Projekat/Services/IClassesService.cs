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
    public interface IClassesService
    {
        IEnumerable<Class> Get();
        IEnumerable<ClassDto> GetAllClasses();
        ClassDto GetClass(int id);
        ClassDto CreateAClass(ClassDto classs);
        string AddStudentToClass(int id, Guid student_id);
        string UpdateClass(int id, Class classForUpdate);
        string DeleteClass(int id);
        ClassDto ClassToDto(Class classs);
        Class DtoToClass(ClassDto classDto);
        List<Student> GetStudents(int id);
        List<Student> GetStudentsWithoutClass();
        IEnumerable<Subject> GetSubjectsWithoutClass();
        List<Teacher> GetTeachers(int id);
        Class AddSubject(int idClass, int idSubject);
    }
}
