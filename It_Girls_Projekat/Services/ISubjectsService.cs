using It_Girls_Projekat.DTOs;
using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace It_Girls_Projekat.Services
{
    public interface ISubjectsService
    {
        IEnumerable<Subject> Get();
        IEnumerable<SubjectDto> GetAllSubjects();
        SubjectDto GetSubject(int id);
        string AddASubject(SubjectDto subject_dto);
        string UpdateSubject([FromUri]int id, [FromBody]SubjectDto subjectDto);
        string AddTeacherToSubject(int sub_id, Guid tea_id);
        SubjectDto DeleteSubject(int id);
        SubjectDto SubjectToDto(Subject subject);
        Subject DtoToSubject(SubjectDto subject_dto);
        SubjectDtoTeacher SubjectDtoTeacher(Subject subject);
        List<Class> GetClassBySubject(Guid idt, int id);
    }
}
