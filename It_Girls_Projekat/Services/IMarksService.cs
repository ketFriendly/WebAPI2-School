using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace It_Girls_Projekat.Services
{
    public interface IMarksService
    {
        IEnumerable<MarkDto> GetStudentMarks(Guid student_id);

        IEnumerable<MarkDto> GetStudentsMarks(Guid parent_id);

        IEnumerable<MarkDto> GetAllMarks(Guid teacher_id);

        Mark PostMark(string teacher_id, string student_id, int subject_id, int mark);

        Mark DtoToMark(MarkDto markDto);

        MarkDto MarkToDto(Mark mark);

        List<MarkDto> GetMarksBySubject(string subject, string id);

        double? GetAverageGrade(string subject, Guid student_id, Guid teacher_id);
    }
}
