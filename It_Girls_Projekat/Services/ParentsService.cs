using It_Girls_Projekat.DTOs;
using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using It_Girls_Projekat.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace It_Girls_Projekat.Services
{
    public class ParentsService:IParentsService
    {
        private IUnitOfWork db;

        public ParentsService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }
        public IEnumerable<Parent> Get()
        {
            IEnumerable<Parent> parents = db.ParentsRepository.Get();
           
            if (parents == null)
            {
                return null;
            }
            
            return parents;
        }
        public IEnumerable<UserDto> GetAllParents()
        {
            IEnumerable <Parent> parents = db.ParentsRepository.Get();
            List<UserDto> user_parents = new List<UserDto>();
            if (parents == null)
            {
                return null;
            }
            foreach (var parent in parents)
            {
                UserDto user = new UserDto()
                {
                    FirstName = parent.FirstName,
                    LastName = parent.LastName,
                    UserName = parent.UserName,
                    Email = parent.Email
                };
                user_parents.Add(user);
            }
            return user_parents;
        }
        public ParentDto GetParent(Guid id)
        {
            Parent parent = db.ParentsRepository.GetByID(id.ToString());
            ParentDto user_parent = new ParentDto();
            if (parent != null)
            {
                user_parent = ParentToParentDto(parent);
                return user_parent;
            }
            return null;
           
        }
        public ParentDto DeleteParent(Guid id)
        {
            Parent parent = db.ParentsRepository.GetByID(id.ToString());
            if (parent == null)
            {
                return null;
            }
            ParentDto user_parent = ParentToParentDto(parent);
            foreach (var student in parent.Students)
            {
                student.Parents.Remove(parent);
            }
            parent.Students = null;
            db.ParentsRepository.Delete(parent);
            db.Save();
            return user_parent;
        }
        public string UpdateParent(Guid id, string firstName, string lastName, string username, string email, string password)
        {
            Parent parent = db.ParentsRepository.GetByID(id.ToString());
            if (parent == null)
            {
                return "There is no parent with id " + id + ".";
            }
            UserDto user = new UserDto();

            if (db.AuthRepository.FindUserByUsername(username) == null)
            {
                parent.FirstName = firstName;
                user.FirstName = firstName;
                parent.LastName = lastName;
                user.LastName = lastName;
                parent.UserName = username;
                user.UserName = username;
                parent.Email = email;
                user.Email = email;
                db.ParentsRepository.Update(parent);
                db.Save();
                return "You updated the parent successfully.";
            }
            return "There is a user with the username " + username + ".";
        }
        public string AddExistingStudent(Guid id_parent, Guid id_student)
        {
            Parent parent = db.ParentsRepository.GetByID(id_parent.ToString());
            Student student = db.StudentsRepository.GetByID(id_student.ToString());
            if (parent == null)
            {
                return "The parent with id " + id_parent + "doesn't exist.";
            }
            if (student == null)
            {
                return "The student with id " + id_student + "doesn't exist.";
            }
            parent.Students.Add(student);
            student.Parents.Add(parent);
            db.ParentsRepository.Update(parent);
            db.StudentsRepository.Update(student);
            
            db.Save();

            return "Student added successfully!";

        }
        public string AddNewStudent(Guid id, UserDto student, string student_pass)
        {
            Parent parent = db.ParentsRepository.GetByID(id.ToString());
            if (parent == null)
            {
                return "The parent with id " + id + "doesn't exist.";
            }
            db.AuthRepository.FindUser(student.UserName, student_pass);

            if (db.AuthRepository.FindUserByUsername(student.UserName) == null)
            {
                Student child = new Student()
                {
                    UserName = student.UserName,
                    FirstName = student.FirstName,
                    LastName = student.LastName
                };
                db.AuthRepository.RegisterStudentUser(child, student_pass);
                parent.Students.Add(child);

                db.ParentsRepository.Update(parent);

                db.Save();
                return "Student added successfully!";
            }
            return "There is a user with that username";
        }
        public string AddBulkParentStudent(UserDto user_parent, string pass, UserDto user_student, string stud_pass)
        {
            if (user_parent == null || user_student == null)
            {
                return null;
            }
            if (db.AuthRepository.FindUserByUsername(user_parent.UserName) != null)
            {
                return "A user with" + user_parent.UserName + "username already exists";
            }
            if (db.AuthRepository.FindUserByUsername(user_student.UserName) != null)
            {
                return "A user with" + user_student.UserName + "username already exists";
            }
            Parent parent = new Parent()
            {
                UserName = user_parent.UserName,
                FirstName = user_parent.FirstName,
                LastName = user_parent.LastName,
                Email = user_parent.Email
            };
            db.AuthRepository.RegisterParentUser(parent, pass);

            Student child = new Student()
            {
                UserName = user_student.UserName,
                FirstName = user_student.FirstName,
                LastName = user_student.LastName
            };

            db.AuthRepository.RegisterStudentUser(child, stud_pass);
            child.Parents.Add(parent);
            parent.Students.Add(child);
            db.ParentsRepository.Update(parent);
            db.StudentsRepository.Update(child);
            db.Save();
            return "Student and parent saved successfully!";

        }
        public ParentDto ChangeEmail(Guid id, string email)
        {
            Parent parent = db.ParentsRepository.ChangeEmail(id, email);
            if (parent == null)
            {
                return null;
            }
            db.ParentsRepository.Update(parent);
            db.Save();
            return ParentToParentDto(parent);
        }
        public ParentDto GetByUserName(string username)
        {
            ApplicationUser user = db.AuthRepository.FindUserByUsername(username);
            ParentDto parentDto = new ParentDto();
            //foreach (var role in user.Roles)
            //{
            //    string name = role.RoleId;
            //    string RoleName = user.Roles.First(r => r.Id == role.RoleId).Name;
            //}
            //string rolee = parent.Roles.(x => x.RoleId).ToString();
            //var roleStore = new RoleStore<IdentityRole>(db.ParentsRepository);
            //var roleMngr = new RoleManager<IdentityRole>(roleStore);

            //var roles = roleMngr.Roles.ToList();
            //if (user != null)
            //{
            //    if (user.Roles.FirstOrDefault(x => x.RoleId == role) != null)
            //    {
            //        parentDto.Username = user.UserName;
            //        parentDto.Surname = user.LastName;
            //        parentDto.Name = user.FirstName;
            //        parentDto.Email = user.Email;
            //        return parentDto;
            //    }
            //}
            return null;
            
        }
        public UserDto ParentToUserDto(Parent parent)
        {
            UserDto user = db.ParentsRepository.ParentToUserDto(parent);
            if (user == null)
            {
                return null;
            }
            return user;
        }
        public ParentDto ParentToParentDto(Parent parent)
        {
            ParentDto user = db.ParentsRepository.ParentToParentDto(parent);
            if (user != null && parent.Students != null)
            {
                foreach (var student in parent.Students)
                {
                    StudentDto studentDto = db.StudentsRepository.StudentToStudentDto(student);
                    user.Students.Add(studentDto);
                }
            }
            return user;
        }
        public Parent ParentDtoToParent(ParentDto parentDto)
        {
            
            Parent parent = db.ParentsRepository.ParentDtoToParent(parentDto);
            //TeachesSubjects ts = new TeachesSubjects();
            if (parentDto.Students != null)
            {
                foreach (var studentt in parentDto.Students)
                {
                    Student student = db.StudentsRepository.StudentDtoToStudent(studentt);
                    parent.Students.Add(student);
                    //ts.Teacher = teacher;
                    //ts.Subject = subjectt;
                    //teacher.Subjects.Add(ts);
                }
            }
            return parent;
        }

}
}