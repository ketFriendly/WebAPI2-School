using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace It_Girls_Projekat.Repositories
{
    public interface ISubjectsRepository:IGenericRepository<Subject>
    {
        SubjectDto SubjectToDto(Subject subject);
        Subject DtoToSubject(SubjectDto subject_dto);
        SubjectDtoTeacher SubjectToTeacherDto(Subject subject);
    }
}
