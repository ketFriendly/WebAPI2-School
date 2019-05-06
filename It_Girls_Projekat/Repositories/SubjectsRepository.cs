using It_Girls_Projekat.Infrastructure;
using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace It_Girls_Projekat.Repositories
{
    public class SubjectsRepository:GenericRepository<Subject>,ISubjectsRepository
    {
        private AuthContext db;
        public SubjectsRepository(AuthContext db) : base(db)
        {
            this.db = db;
        }
        public SubjectDto SubjectToDto(Subject subject)
        {
            SubjectDto dto = new SubjectDto();
            if (subject != null)
            {
                dto.SubjectDtoId = subject.IdSubject;
                dto.SubjectName = subject.Name;
                dto.WeeklyClassNo = subject.WeeklyClassNo;
                return dto;
            }
            return null;
        }
        public Subject DtoToSubject(SubjectDto subject_dto)
        {
           

            if (subject_dto != null)
            {
                Subject subject = new Subject();
                subject.Name = subject_dto.SubjectName;
                subject.WeeklyClassNo = subject_dto.WeeklyClassNo;
                //if (subject_dto.Teachers != null)
                //{
                //    foreach (var teacher in subject_dto.Teachers)
                //    {
                //        ts.Teacher = teacher;
                //        ts.Subject = subject;
                //        tss.Add(ts);
                //    }
                //    subject.Teachers = tss;
                //}
                return subject;
            }
            return null;
        }
        public SubjectDtoTeacher SubjectToTeacherDto(Subject subject)
        {
            SubjectDtoTeacher dto = new SubjectDtoTeacher();
            if (subject != null)
            {
                dto.SubjectDtoTeacherId = subject.IdSubject;
                dto.SubjectName = subject.Name;
                dto.WeeklyClassNo = subject.WeeklyClassNo;
                return dto;
            }
            return null;
        }
    }
}