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
    public class StudentsService : IStudentsService
    {
        private IUnitOfWork db;

        public StudentsService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }
        public IEnumerable<Student> GetAll()
        {
            IEnumerable<Student> students = db.StudentsRepository.Get();
            if (students == null)
            {
                return null;
            }
            return students;
        }
        public IEnumerable<StudentDto> GetAllStudents()
        {
            IEnumerable<Student> students = db.StudentsRepository.Get();
            List<StudentDto> user_students = new List<StudentDto>();
            if (students == null)
            {
                return null;
            }
            foreach (var student in students)
            {
                StudentDto user = StudentToStudentDto(student);
                user_students.Add(user);
            }
            return user_students;
        }
        public StudentDto GetStudent(Guid id)
        {
            Student student = db.StudentsRepository.GetByID(id.ToString());
            if (student == null)
            {
                return null;
            }
            StudentDto user = StudentToStudentDto(student);
            return user;
        }
        public UserDto DeleteStudent(Guid id)
        {
            Student student = db.StudentsRepository.GetByID(id.ToString());
            if (student == null)
            {
                return null;
            }
            foreach (var parent in student.Parents)
            {
                parent.Students.Remove(student);
            }
            string idd = id.ToString();
            IEnumerable<Mark> marks = db.MarksRepository.Get();
            IEnumerable<Mark> markk = marks.Where(mark => mark.IdStudent == idd);
            List<Mark> list = new List<Mark>();
            list = markk.ToList();
            if (list.Count == 0)
            {
                return null;
            }
            foreach (var item in list)
            {
                db.MarksRepository.Delete(item);
            }
            db.StudentsRepository.Delete(id.ToString());
            db.Save();
            return StudentToUserDto(student);
        }
        public StudentDto ChangeEmail(Guid id, string email)
        {
            Student student = db.StudentsRepository.GetByID(id.ToString());
            if (student == null)
            {
                return null;
            }
            bool valid = db.StudentsRepository.IsValidEmail(email);
            if (valid == true)
            {
                student.Email = email;
                db.StudentsRepository.Update(student);
                db.Save();
            }
            return StudentToStudentDto(student);
        }
            
        public string UpdateStudent(Guid id, string firstName, string lastName, string username, string password)
        {
            Student student = db.StudentsRepository.GetByID(id.ToString());
            if (student == null)
            {
                return "There is no user with this id";
            }
            ApplicationUser user = db.AuthRepository.FindUserByUsername(username);
            if (user != null)
            {
                return "An user with this username already exists.";
            }
            student.FirstName = firstName;
            student.LastName = lastName;
            student.UserName = username;
            student.PasswordHash = password;
            db.StudentsRepository.Update(student);
            db.Save();
            return "Student successfully updated.";
        }
        public UserDto GetByUserName(string username)
        {
            ApplicationUser user = db.AuthRepository.FindUserByUsername(username);
            if (user == null)
            {
                return null;
            }
            UserDto userdto = new UserDto()
            {
                UserName = user.UserName,
                LastName = user.LastName,
                FirstName = user.FirstName,
                Email = user.Email
            };
            return userdto;
        }
        public string AddExistingParent(Guid id_parent, Guid id_student)
        {
            Student student = db.StudentsRepository.GetByID(id_student.ToString());
            Parent parent = db.ParentsRepository.GetByID(id_parent.ToString());
            if (student == null)
            {
                return "There is no students with this id";
            }
            if (parent == null)
            {
                return "There is no parents with this id";
            }
            student.Parents.Add(parent);
            parent.Students.Add(student);
            db.StudentsRepository.Update(student);
            db.ParentsRepository.Update(parent);
            db.Save();
            return "Parent added successfully";

        }
        public string AddNewParent(Guid id, UserDto parent, string parent_pass)
        {
            Student student = db.StudentsRepository.GetByID(id.ToString());
            if (student == null)
            {
                return "There is no students with this id";
            }
            if (db.AuthRepository.FindUserByUsername(parent.UserName) == null)
            {
                Parent par = new Parent()
                {
                    UserName = parent.UserName,
                    FirstName = parent.FirstName,
                    LastName = parent.LastName,
                    Email = parent.Email
                };
                db.AuthRepository.RegisterParentUser(par, parent_pass);
                student.Parents.Add(par);

                db.StudentsRepository.Update(student);

                db.Save();
                return "Parent added successfully!";
            }
            return "There is a user with that username";
            //db.AuthRepository.FindUser(parent.UserName, parent_pass);
            //parent.Password = parent_pass;
            //Parent parent_user = db.ParentsRepository.UserDtoToParent(parent);
            //parent_user.Students.Add(student);
            //db.AuthRepository.RegisterParentUser(parent_user, parent_pass);
            //student.Parents.Add(parent_user);
            //db.StudentsRepository.Update(student);
            //db.Save();
            //return "Parent added successfully.";



        }
        //public string AddBulkParentStudent(UserDto user_parent, string pass, UserDto user_student, string stud_pass)
        //{
        //    if (user_parent == null || user_student == null)
        //    {
        //        return null;
        //    }
        //    if (db.AuthRepository.FindUserByUsername(user_parent.UserName) != null)
        //    {
        //        return "A user with" + user_parent.UserName + "username already exists";
        //    }
        //    if (db.AuthRepository.FindUserByUsername(user_student.UserName) != null)
        //    {
        //        return "A user with" + user_student.UserName + "username already exists";
        //    }
        //    Parent parent = new Parent()
        //    {
        //        UserName = user_parent.UserName,
        //        FirstName = user_parent.FirstName,
        //        LastName = user_parent.LastName,
        //        Email = user_parent.Email
        //    };
        //    db.AuthRepository.RegisterParentUser(parent, pass);

        //    Student child = new Student()
        //    {
        //        UserName = user_student.UserName,
        //        FirstName = user_student.FirstName,
        //        LastName = user_student.LastName
        //    };

        //    db.AuthRepository.RegisterStudentUser(child, stud_pass);
        //    child.Parents.Add(parent);
        //    parent.Students.Add(child);
        //    db.ParentsRepository.Update(parent);
        //    db.StudentsRepository.Update(child);

        //    db.Save();
        //    return "Student and parent saved successfully!";
        //}

        public UserDto StudentToUserDto(Student student)
        {
            if (student == null)
            {
                return null;
            }
            UserDto user = db.StudentsRepository.StudentToUserDto(student);
            return user;
        }
        public Student UserDtoToStudent(UserDto user)
        {
            if (user == null)
            {
                return null;
            }
            Student student = db.StudentsRepository.UserDtoToStudent(user);
            return student;
        }
        
        public StudentDto StudentToStudentDto(Student student)
        {
            StudentDto user = db.StudentsRepository.StudentToStudentDto(student);
            ParentDto parentDto = new ParentDto();
            if (user == null)
            {
                return null;
            }
            /*komentar*/
            //if (student.Parents.Count() > 0)
            //{
            //    foreach (var parent in student.Parents)
            //    {
            //        parentDto = db.ParentsRepository.ParentToParentDto(parent);
            //        user.Parents.Add(parentDto);
            //    }
            //}
            //if (student.Marks.Count() != 0)
            //{
            //    foreach (var mark in student.Marks)
            //    {
            //        MarkDto markDto = db.MarksRepository.MarkToDto(mark);
            //        user.Marks.Add(markDto);
            //    }
            //}
            //user.Class = db.ClassesRepository.ClassToDto(student.Class);
            /*komentar*/
            return user;
        }
        public Student StudentToStudentDto(StudentDto studentDto)
        {
            Student student = db.StudentsRepository.StudentDtoToStudent(studentDto);
            if (student == null)
            {
                return null;
            }
            student.Class = db.ClassesRepository.DtoToClass(studentDto.Class);
            if (studentDto.Marks != null)
            {
                foreach (var markk in studentDto.Marks)
                {
                    Mark mark = db.MarksRepository.DtoToMark(markk);
                    student.Marks.Add(mark);
                }
            }
            if (studentDto.Parents != null)
            {
                foreach (var parentt in studentDto.Parents)
                {
                    Parent parent = db.ParentsRepository.ParentDtoToParent(parentt);
                    student.Parents.Add(parent);
                }
            }
            return student;
        }
        public List<Student> GetByParent(Guid id)
        {
            string ID = id.ToString();
            IEnumerable<Student> allStudents = db.StudentsRepository.Get();
            List<Student> children = new List<Student>();
            foreach (Student item in allStudents)
            {
                if (item.Parents.Count != 0)
                {
                    foreach (var parent in item.Parents)
                    {
                        if (parent.Id == ID)
                        {
                            children.Add(item);
                        }
                    }
                }
            }
            return children;
        }
    }
}