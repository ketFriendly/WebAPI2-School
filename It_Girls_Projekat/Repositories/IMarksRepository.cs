using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace It_Girls_Projekat.Repositories
{
    public interface IMarksRepository:IGenericRepository<Mark>
    {
        Mark DtoToMark(MarkDto markDto);
        MarkDto MarkToMarkDto(Mark mark, Teacher teacher);
        MarkDto MarkToDto(Mark mark);
        //DataSet GetDataSet(Mark mark, Teacher teacher);
        //string GetHtml(DataSet dataSet);
        //void SendMarkByEmail(string htmlString, string parentEmail);
        Mark MarkDtoToMark(MarkDto markDto, Teacher teacher, Student student);
    }
}
