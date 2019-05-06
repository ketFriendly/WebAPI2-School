using It_Girls_Projekat.DTOs;
using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using It_Girls_Projekat.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace It_Girls_Projekat.Services
{
    public class SubjectsService:ISubjectsService
    {
        private IUnitOfWork db;

        public SubjectsService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }
        public IEnumerable<Subject> Get()
        {
            IEnumerable<Subject> subjects = db.SubjectsRepository.Get();
            if (subjects == null)
            {
                return null;
            }
            
            return subjects;
        }
        public IEnumerable<SubjectDto> GetAllSubjects()
        {
            IEnumerable<Subject> subjects = db.SubjectsRepository.Get();
            if (subjects == null)
            {
                return null;
            }
            List<SubjectDto> dtos = new List<SubjectDto>();
            foreach (var subject in subjects)
            {
                SubjectDto dto = SubjectToDto(subject);
                dtos.Add(dto);
            }
            return dtos;
        }
        public SubjectDto GetSubject(int id)
        {
            Subject subject = db.SubjectsRepository.GetByID(id);
            if (subject == null)
            {
                return null;
            }
            SubjectDto dto = SubjectToDto(subject);
            return dto;
        }
        public string AddASubject(SubjectDto subject_dto)
        {
            if (subject_dto == null)
            {
                return null;
            }
            Subject subject = DtoToSubject(subject_dto);
            db.SubjectsRepository.Insert(subject);
            db.Save();

            return "Subject added successfully";
        }
        public string UpdateSubject(int id, SubjectDto subjectDto )
        {
            Subject subject = db.SubjectsRepository.GetByID(id);
            TeachesSubjects ts = new TeachesSubjects();
            if (subject == null)
            {
                return null;
            }
            if (subjectDto.Teachers != null)
            {
                foreach (var teacher in subjectDto.Teachers)
                {
                    ApplicationUser user= db.AuthRepository.FindUserByUsername(teacher.Username);
                    if (user != null)
                    {
                        return "There is a user with this (" + teacher.Username + ") username.";
                    }
                    Teacher teacherr = new Teacher()
                    {
                        FirstName = teacher.Name,
                        LastName = teacher.Surname,
                        UserName = teacher.Username,
                        Email = teacher.Email
                    };
                    ts.Teacher = teacherr;
                    ts.Subject = subject;
                    teacherr.Subjects.Add(ts);
                    subject.Teachers.Add(ts);

                    db.teach_subj_repo.Insert(ts);
                    db.TeachersRepository.Update(teacherr);
                    //db.SubjectsRepository.Update(subject);
                    db.Save();    
                }
            }
            
            subject.Name = subjectDto.SubjectName;
            subject.WeeklyClassNo = subjectDto.WeeklyClassNo;
            db.SubjectsRepository.Update(subject);
            
            db.Save();
            return "Subject updated successfully";
        }
        public string AddTeacherToSubject(int sub_id, Guid tea_id)
        {
            Subject subject = db.SubjectsRepository.GetByID(sub_id);
            if (subject == null)
            {
                return "There is no subjects with this id.";
            }
            Teacher teacher = db.TeachersRepository.GetByID(tea_id.ToString());
            if (teacher == null)
            {
                return "There is no teachers with this id.";
            }
            TeachesSubjects ts = new TeachesSubjects();
            ts.IdSubject = subject.IdSubject;
            ts.Subject = subject;
            ts.IdTeacher = Guid.Parse(teacher.Id);
            ts.Teacher = teacher;

            subject.Teachers.Add(ts);
            teacher.Subjects.Add(ts);

            db.teach_subj_repo.Insert(ts);
            db.SubjectsRepository.Update(subject);
            db.TeachersRepository.Update(teacher);
            db.Save();

            return "Teacher added successfully";
        }
        public SubjectDto DeleteSubject(int id)
        {
            Subject subject = db.SubjectsRepository.GetByID(id);
            if (subject == null)
            {
                return null;
            }
            db.SubjectsRepository.Delete(id);
            db.Save();
            return SubjectToDto(subject);
        }
        public List<Class> GetClassBySubject(Guid idt,int id)
        {
            IEnumerable<Class> allEnumerClasses = db.ClassesRepository.Get();
            List<Class> allClasses = allEnumerClasses.ToList();
            List <Class> classes = new List<Class>();
            if (allClasses == null)
            {
                return null;
            }
            for (int i = 0; i < allClasses.Count; i++)
            {
                if (allClasses[i].TeachesSubjects != null)
                {
                    foreach (var item in allClasses[i].TeachesSubjects)
                    {
                        if (item.IdSubject == id && item.IdTeacher == idt)
                        {
                            classes.Add(allClasses[i]);
                        }
                    }
                }
            }
            return classes;
            
        }
        public SubjectDto SubjectToDto (Subject subject)
        {
            TeacherDto teacher = new TeacherDto();
            SubjectDto dto = db.SubjectsRepository.SubjectToDto(subject);
            if (dto != null)
            {
                if (subject.Teachers != null)
                {
                    foreach (var teacherr in subject.Teachers)
                    {
                        teacher = db.TeachersRepository.TeachertoTeacherDto(teacherr.Teacher);
                        dto.Teachers.Add(teacher);
                    }
                }
                return dto;
            }
            return null;
        }
        public Subject DtoToSubject(SubjectDto subject_dto)
        {
            TeachesSubjects ts = new TeachesSubjects();
            List<TeachesSubjects> tss = new List<TeachesSubjects>();
            Subject subject = db.SubjectsRepository.DtoToSubject(subject_dto);
            if (subject != null)
            {
                if (subject.Teachers.Count() != 0)
                {
                    foreach (var teacher in subject_dto.Teachers)
                    {
                        ts.Teacher = db.TeachersRepository.TeacherDtoToTeacher(teacher);
                        ts.Subject = subject;
                        tss.Add(ts);
                    }
                    subject.Teachers = tss;
                }
                return subject;
            }
            return null;
        }

        public SubjectDtoTeacher SubjectDtoTeacher(Subject subject)
        {
            Teacher teacher = new Teacher();
            SubjectDtoTeacher dto = db.SubjectsRepository.SubjectToTeacherDto(subject);
            if (dto != null)
            {
                if (subject.Teachers != null)
                {
                    foreach (var teacherr in subject.Teachers)
                    {
                        //teacher = db.TeachersRepository.TeachertoTeacherDto(teacherr.Teacher);
                        dto.Teachers.Add(teacherr.Teacher);
                    }
                }
                return dto;
            }
            return null;
        }
    }
}