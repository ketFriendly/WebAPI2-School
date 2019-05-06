using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using It_Girls_Projekat.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Configuration;
using System.Web.Http;
using System.Net.Http;
using System.Security.Claims;
using System.ServiceModel.Channels;

namespace It_Girls_Projekat.Services
{
    public class MarksService:IMarksService
    {
        private IUnitOfWork db;

        public MarksService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }
        public IEnumerable<MarkDto> GetStudentMarks(Guid student_id)
        {
            Student student = db.StudentsRepository.GetByID(student_id.ToString());
            if (student == null)
            {
                return null;
            }
            List<Mark> marks = student.Marks.ToList();
            List<MarkDto> markss = new List<MarkDto>();
            if (marks.Count != 0)
            {
                foreach (var mark in marks)
                {
                    MarkDto dto = new MarkDto();
                    int tsId = Convert.ToInt32(mark.IdTeachesSubject);
                    TeachesSubjects ts = db.teach_subj_repo.GetByID(tsId);
                    Teacher teach = db.TeachersRepository.GetByID(ts.IdTeacher.ToString());
                    dto.TeachersName = teach.FirstName +" "+ teach.LastName;
                    dto.Subject = mark.TeacherAndSubject.Subject.Name;
                    dto.Mark = mark.Marks.ToString();
                    markss.Add(dto);
                }
                return markss;
            }
            return null;
        }
        public IEnumerable<MarkDto> GetStudentsMarks(Guid parent_id)
        {
            Parent parent = db.ParentsRepository.GetByID(parent_id.ToString());
            List<Guid> students = new List<Guid>();
            if (parent!=null)
            {
                foreach (var student in parent.Students)
                {
                    students.Add(Guid.Parse(student.Id));
                }
            }
            List<MarkDto> marks = new List<MarkDto>();
            foreach (var id in students)
            {
                IEnumerable<MarkDto> markss = GetStudentMarks(id);
                Student stud = db.StudentsRepository.GetByID(id.ToString());
                foreach (var mark in markss)
                {
                    mark.StudentsName = stud.FirstName + " " + stud.LastName;
                }
               marks.AddRange(markss);
            }
            return marks;
        }
        public IEnumerable<MarkDto> GetAllMarks(Guid teacher_id)
        {
            Teacher teacher = db.TeachersRepository.GetByID(teacher_id.ToString());
            if (teacher == null)
            {
                return null;
            }
            IEnumerable<Mark> allmarks = db.MarksRepository.Get();
            List<Mark> teachersMarks = new List<Mark>();
            List<MarkDto> teachersDtos = new List<MarkDto>();
            foreach (var mark in allmarks)
            {
                if (mark.TeacherAndSubject.IdTeacher == teacher_id)
                {
                    teachersMarks.Add(mark);
                }
            }
            if (teachersMarks.Count != 0 )
            {
                foreach (var mark in teachersMarks)
                {
                    Student stud = db.StudentsRepository.GetByID(mark.Student.Id.ToString());
                    MarkDto dto = new MarkDto();
                    dto.Mark = mark.Marks.ToString();
                    dto.Subject = mark.TeacherAndSubject.Subject.Name;
                    dto.StudentsName =stud.FirstName + " " + stud.LastName;
                    teachersDtos.Add(dto);
                }
            }
            return teachersDtos;
        }
        
        public Mark PostMark(string teacher_id, string student_id,int subject_id, int mark)
        {
            Teacher teacher = db.TeachersRepository.GetByID(teacher_id);
            Student student = db.StudentsRepository.GetByID(student_id);
            Subject subject = db.SubjectsRepository.GetByID(subject_id);
            TeachesSubjects ts = new TeachesSubjects();
            Mark markk = new Mark();
            if (teacher == null || student == null || subject == null)
            {
                return null;
            }
            List<Parent> parents = student.Parents.ToList(); 
            ts.IdSubject = subject_id;
            ts.IdTeacher = Guid.Parse(teacher_id);
            if (mark > 0 && mark < 6)
            {
                markk.Student = student;
                markk.TeacherAndSubject = ts;
                markk.IdStudent = student_id;
                markk.Marks = (Marks)mark;
                student.Marks.Add(markk);
                db.StudentsRepository.Update(student);
                db.MarksRepository.Insert(markk);
                db.Save();
            }

            if (parents.Count() != 0)
            {
                foreach (var item in parents)
                {
                    PostSendMail("mladenovic.sanja.sv@gmail.com", item.FirstName, markk);
                }
            }
            return markk;
        }

        public void PostSendMail(string email, string parents_name, Mark mark)
        {
            string subject = "Student's report card";
            string body = "Email body";
            body += $"<p>Dear {parents_name} ,<br/>" +
                       $"We are sending you your kids report card {DateTime.Now}.</p>" +
                       $"<br/>" +
                       $"<br/>";

            body += "<table border='1'><td><b><center> Student's name </center></b></td><td><b><center> Student's surname </center></b></td><td><b><center> Mark </center></b></td>";


            body += "<tr><td>" + mark.Student.FirstName + "</td><td>" + mark.Student.LastName + "</td><td><center>" + mark.Marks.ToString() + "</center></td></tr>";


            body += "</table>";

            body += $"<br/>" +
                    $"<br/>" +
                    $"Sincerely,<br/>" +
                    $"Sanja, Sanja's-School";
            string FromMail = ConfigurationManager.AppSettings["from"];
            string emailTo = "mladenovic.sanja.sv@gmail.com";
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["smtpServer"]);
            mail.From = new MailAddress(FromMail);
            mail.To.Add(emailTo);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpServer.Port = int.Parse(ConfigurationManager.AppSettings["smtpPort"]);
            SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["from"], ConfigurationManager.AppSettings["password"]);
            SmtpServer.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["smtpSsl"]);
            SmtpServer.Send(mail);
        }
        public Mark DtoToMark (MarkDto markDto)
        {
            Mark mark = db.MarksRepository.DtoToMark(markDto);
            return mark;
        }
        public MarkDto MarkToDto (Mark mark)
        {
            MarkDto dto = db.MarksRepository.MarkToDto(mark);            
            return dto;
        }
        public MarkDto MarkToMarkDto(Mark mark, string teacher)
        {
            Teacher teach = db.TeachersRepository.GetByID(teacher);
            MarkDto dto = db.MarksRepository.MarkToMarkDto(mark, teach);
            return dto;
        }
        public List<MarkDto> GetMarksBySubject(string subject, string id)
        {
            IEnumerable<Mark> marks = db.MarksRepository.Get();
            IEnumerable<Subject> subs = db.SubjectsRepository.Get();
            Subject sub = subs.FirstOrDefault(x => x.Name == subject);
            Guid guid = Guid.Parse(id);
            IEnumerable<Mark> marksByTeacher = marks.Select(x =>
            { x.TeacherAndSubject.IdTeacher = guid;
              return x; });
            List<Mark> marksBySubject = new List<Mark>();
            foreach (var mark in marksByTeacher)
            {
                bool isEqual = mark.TeacherAndSubject.Subject.Equals(sub);
                if (isEqual == true)
                {
                    marksBySubject.Add(mark);
                }
            }
            List<MarkDto> marksDtos = new List<MarkDto>();
            foreach (var mark in marksBySubject)
            {
                string teacherId = mark.TeacherAndSubject.IdTeacher.ToString();
                MarkDto dto = MarkToMarkDto(mark, teacherId);
                marksDtos.Add(dto);
            }
            return marksDtos;
        }
        public double? GetAverageGrade(string subject, Guid student_id, Guid teacher_id)
        {
            Student stud = db.StudentsRepository.GetByID(student_id.ToString());
            IEnumerable<MarkDto> markDtos = GetStudentMarks(student_id);
            if (markDtos == null)
            {
                return null;
            }
            List<int> mark_int = new List<int>();
            foreach (var mark in markDtos)
            {
                Marks parsed; 
                bool isParsed = Marks.TryParse(mark.Mark, out parsed);
                if (isParsed)
                {
                    int markk = Convert.ToInt32(parsed);
                    mark_int.Add(markk);
                }
            }
            
            double? average = mark_int.Average();
            return average;
        }
        
    }
}