using It_Girls_Projekat.Infrastructure;
using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace It_Girls_Projekat.Repositories
{
    public class MarksRepository:GenericRepository<Mark>, IMarksRepository
    {
        private System.Data.DataSet dataSet;
        private AuthContext db;
        public MarksRepository(AuthContext db) : base(db)
        {
            this.db = db;
        }
        public Mark DtoToMark(MarkDto markDto)
        {
            Mark mark = new Mark();
            if (markDto == null)
            {
                return null;
            }
            Marks markEnum = new Marks();
            bool parse = Marks.TryParse(markDto.Mark, out markEnum);
            if (parse == true)
            {
                mark.Marks = markEnum;
            }
            string[] strings = markDto.StudentsName.Split(null);
            if (strings != null)
            {
                mark.Student.FirstName = strings[0];
                mark.Student.LastName = strings[1];
            }
            string[] teachersName = markDto.TeachersName.Split(null);
            if (teachersName != null)
            {
                mark.TeacherAndSubject.Teacher.FirstName = teachersName[0];
                mark.TeacherAndSubject.Teacher.LastName = teachersName[1];
            }
            mark.TeacherAndSubject.Subject.Name = markDto.Subject;
            return mark;
        }
        public MarkDto MarkToDto(Mark mark)
        {
            MarkDto dto = new MarkDto();
            
            dto.TeachersName = mark.TeacherAndSubject.Teacher.FirstName + mark.TeacherAndSubject.Teacher.LastName;
            dto.StudentsName = mark.Student.FirstName + mark.Student.LastName;
            dto.Subject = mark.TeacherAndSubject.Subject.Name;
            dto.Mark = mark.Marks.ToString();
            return dto;
        }
        public MarkDto MarkToMarkDto(Mark mark, Teacher teacher)
        {
            MarkDto dto = new MarkDto();

            dto.TeachersName = teacher.FirstName + " " + teacher.LastName;
            dto.StudentsName = mark.Student.FirstName + mark.Student.LastName;
            dto.Subject = mark.TeacherAndSubject.Subject.Name;
            dto.Mark = mark.Marks.ToString();
            return dto;
        }
        public Mark MarkDtoToMark(MarkDto markDto,Teacher teacher, Student student)
        {
             
            Mark mark = new Mark();
            if (markDto == null)
            {
                return null;
            }
            Marks markEnum = new Marks();
            bool parse = Marks.TryParse(markDto.Mark, out markEnum);
            if (parse == true)
            {
                mark.Marks = markEnum;
            }
            //string[] strings = markDto.StudentsName.Split(null);
            //if (strings != null)
            //{
                mark.Student = student;

            //}
            //string[] teachersName = markDto.TeachersName.Split(null);
            //if (teachersName != null)
            //{
            Guid guid = Guid.Parse(teacher.Id);
                mark.TeacherAndSubject.IdTeacher = guid;
                
            //}
            mark.TeacherAndSubject.Subject.Name = markDto.Subject;
            return mark;
        }

    }
}