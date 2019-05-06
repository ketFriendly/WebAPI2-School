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
    public class StudentsRepository: GenericRepository<Student>,IStudentsRepository
    {
        private AuthContext db;
        public StudentsRepository(AuthContext db) : base(db)
        {
            this.db = db;
        }
        public Student GetByUserName(string username)
        {
            Student student = Get().FirstOrDefault(x => x.UserName == username);
            if (student == null)
            {
                return null;
            }
            return student;
        }
        public Student ChangeEmail(Guid id, string email)
        {
            Student student = GetByID(id);
            if (student == null || email == null)
            {
                return null;
            }
            bool valid = IsValidEmail(email);
            if (valid == false)
            {
                return null;
            }
            student.Email = email;
            return student;
        }
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public UserDto StudentToUserDto(Student student)
        {
            if (student == null)
            {
                return null;
            }
            UserDto user = new UserDto()
            {
                UserName = student.UserName,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Password = student.PasswordHash
            };
            return user;
        }
        public Student UserDtoToStudent(UserDto user)
        {
            if (user == null)
            {
                return null;
            }
            Student student = new Student()
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PasswordHash = user.Password
            };
            return student;
        }
        public StudentDto StudentToStudentDto(Student student)
        {
            if (student == null)
            {
                return null;
            }
            StudentDto user = new StudentDto();
            //user.Class = student.Class;
            user.Username = student.UserName;
            user.Name = student.FirstName;
            user.Surname = student.LastName;
            user.Email = student.Email;
            return user;
        }
        public Student StudentDtoToStudent(StudentDto studentDto)
        {
            if (studentDto == null)
            {
                return null;
            }
            Student student = new Student();
            student.UserName = studentDto.Username;
            student.FirstName = studentDto.Name;
            student.LastName = studentDto.Surname;
            student.Email = studentDto.Email;
            
            return student;
        }

    }
}