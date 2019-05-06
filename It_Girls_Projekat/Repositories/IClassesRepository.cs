using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace It_Girls_Projekat.Repositories
{
    public interface IClassesRepository:IGenericRepository<Class>
    {
        ClassDto ClassToDto(Class classs);
        Class DtoToClass(ClassDto classDto);
        ClassDto ClassToClassDto(Class classs, List<StudentDto> students);
        Class ClassDtoToClass(ClassDto classDto, List<Student> students);
    }
}
