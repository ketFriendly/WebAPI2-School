using It_Girls_Projekat.DTOs;
using It_Girls_Projekat.Infrastructure;
using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace It_Girls_Projekat.Repositories
{
    public class TeachersRepository:GenericRepository<Teacher>,ITeachersRepository
    {
        private AuthContext db;
        public TeachersRepository(AuthContext db) : base(db)
        {
            this.db = db;
        }
        //public Teacher GetByUserName(string username)
        //{
        //    ICollection<ApplicationUser> users 
        //    Teacher teacher = FirstOrDefault(x => x.UserName == username);
        //    if (teacher == null)
        //    {
        //        return null;
        //    }
        //    return teacher;
        //}
        public Teacher ChangeEmail(Guid id, string email)
        {
            Teacher teacher = GetByID(id.ToString());
            if (teacher == null || email == null)
            {
                return null;
            }
            bool? valid = IsValidEmail(email);
            if (valid == false || valid == null)
            {
                return null;
            }
            teacher.Email = email;
            return teacher;
        }
        public bool? IsValidEmail(string email)
        {
            if (email == null)
            {
                return null;
            }
            bool valid = email.EndsWith(".com") && email.Contains("@");
            return valid;
        }
        public UserDto TeacherToUserDto(Teacher teacher)
        {
            if (teacher == null)
            {
                return null;
            }
            UserDto user = new UserDto()
            {
                UserName = teacher.UserName,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Email = teacher.Email,
                Password = teacher.PasswordHash
            };
            return user;
        }
        public Teacher UserDtoToTeacher(UserDto user)
        {
            if (user == null)
            {
                return null;
            }
            Teacher teacher = new Teacher()
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PasswordHash = user.Password
            };
            return teacher;
        }
        public TeacherDto TeachertoTeacherDto(Teacher teacher)
        {
            if (teacher == null)
            {
                return null;
            }
            TeacherDto user = new TeacherDto();
            //if (teacher.Subjects != null)
            //{
            //    foreach (var subject in teacher.Subjects)
            //    {
            //        SubjectDto subjectDto = SubjectToDto(subject.Subject);
            //        user.Subjects.Add(subjectDto);
            //    }
            //}
            user.Username = teacher.UserName;
            user.Name = teacher.FirstName;
            user.Surname = teacher.LastName;
            user.Email = teacher.Email;
            return user;
        }
        public Teacher TeacherDtoToTeacher(TeacherDto teacherDto)
        {
            if (teacherDto == null)
            {
                return null;
            }
            Teacher teacher = new Teacher();
            teacher.UserName = teacherDto.Username;
            teacher.FirstName = teacherDto.Name;
            teacher.LastName = teacherDto.Surname;
            teacher.Email = teacherDto.Email;
            //TeachesSubjects ts = new TeachesSubjects();
            //if (teacherDto.Subjects != null)
            //{
            //    foreach (var subject in teacherDto.Subjects)
            //    {
            //        Subject subjectt = db.SubjectsRepository.DtoToSubject(subject);
            //        ts.Teacher = teacher;
            //        ts.Subject = subjectt;
            //        teacher.Subjects.Add(ts);
            //    }
            //}
            return teacher;
        }
    }
}