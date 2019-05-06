using It_Girls_Projekat.DTOs;
using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using It_Girls_Projekat.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace It_Girls_Projekat.Services
{
    public class TeachersService:ITeachersService
    {
        private IUnitOfWork db;

        public TeachersService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }
        public IEnumerable<Teacher> Get()
        {
            IEnumerable<Teacher> teachers = db.TeachersRepository.Get();
            if (teachers == null)
            {
                return null;
            }
            return teachers;
        }
        public IEnumerable<TeacherDto> GetAllTeachers()
        {
            IEnumerable<Teacher> teachers = db.TeachersRepository.Get();
            List<TeacherDto> user_teachers = new List<TeacherDto>();
            if (teachers == null)
            {
                return null;
            }
            foreach (var teacher in teachers)
            {
                TeacherDto user = TeachertoTeacherDto(teacher);
                user_teachers.Add(user);
            }
            return user_teachers;
        }
        public TeacherDto GetTeacher(Guid id)
        {
            Teacher teacher = db.TeachersRepository.GetByID(id.ToString());
            if (teacher == null)
            {
                return null;
            }
            TeacherDto user = TeachertoTeacherDto(teacher);
            return user;
        }
        public List<SubjectDto> GetSubjects(Guid id)
        {
            var Id = id.ToString();
            Teacher teacher = db.TeachersRepository.GetByID(Id);
            //List<int> subjectIds = new List<int>();
            //List<TeachesSubjects> ts = new List<TeachesSubjects>();
            List<SubjectDto> subjectDtos= new List<SubjectDto>();
            List<Subject> subjects = new List<Subject>();
            if (teacher != null)
            {
                foreach (var item in teacher.Subjects)
                {
                    Subject subject = db.SubjectsRepository.GetByID(item.IdSubject);
                    subjects.Add(subject);
                    SubjectDto dto = db.SubjectsRepository.SubjectToDto(subject);
                    subjectDtos.Add(dto);
                }            
                return subjectDtos;
                
            }
            return null;
            
        }
        public List<Class> GetTeachersClasses(Guid id)
        {
            IEnumerable<Class> allClasses = db.ClassesRepository.Get();
            List<Class> classes = new List<Class>();
            if (allClasses != null)
            {
                foreach (var cl in allClasses)
                {
                    foreach (var item in cl.TeachesSubjects)
                    {
                        if (item.IdTeacher == id)
                        {
                            classes.Add(cl);
                        }
                    }
                }
                return classes;
            }
            return null;
        }
       
        public TeacherDto DeleteTeacher(Guid id)
        {
            string ID = id.ToString();
            Teacher teacher = db.TeachersRepository.GetByID(ID);
            if (teacher == null)
            {
                return null;
            }
            //var ts = db.teach_subj_repo.Get();
            List<int> ids = new List<int>();
            if (teacher.Subjects != null)
            {
                foreach (var teach in teacher.Subjects)
                {
                    if(teach.IdTeacher == id)
                    {
                        ids.Add(teach.IdTeachesSubject);
                    }
                }
            }
            if (ids != null)
            {
                foreach (var item in ids)
                {
                    db.teach_subj_repo.Delete(item);
                }
            }
            db.TeachersRepository.Delete(teacher);
            db.Save();
            return TeachertoTeacherDto(teacher);
        }
        public TeacherDto ChangeEmail(Guid id, string email)
        {
            Teacher teacher = db.TeachersRepository.ChangeEmail(id,email);
            if (teacher == null)
            {
                return null;
            }
            db.TeachersRepository.Update(teacher);
            db.Save();
            //bool? valid = db.TeachersRepository.IsValidEmail(email);
            //if (valid == true)
            //{
            //    teacher.Email = email;
            //    db.TeachersRepository.Update(teacher);
            //    db.Save();
            //}
            return TeachertoTeacherDto(teacher);
        }

        public string UpdateTeacher(Guid id, string firstName, string lastName, string username, string password, string email)
        {
            string ID = id.ToString();
            Teacher teacher = db.TeachersRepository.GetByID(ID);
            if (teacher == null)
            {
                return "There is no user with this id";
            }
            ApplicationUser user = db.AuthRepository.FindUserByUsername(username);
            if (user != null && user.Id !=ID)
            {
                return "An user with this username already exists.";
            }
            bool? valid = db.TeachersRepository.IsValidEmail(email);
            if (valid == false || valid == null)
            {
                return "There is an user with this email address.";
            }
            teacher.FirstName = firstName;
            teacher.LastName = lastName;
            teacher.UserName = username;
            teacher.Email = email;
            teacher.PasswordHash = password;
            db.TeachersRepository.Update(teacher);
            db.Save();
            return "Teacher successfully updated.";
        }
        public TeacherDto GetByUserName(string username)
        {
            ApplicationUser user = db.AuthRepository.FindUserByUsername(username);
            Task<IList<string>>list = db.AuthRepository.FindRoles(user.Id);
            if (user != null)
            {
                if (user.Roles.ToString() == "Teacher")
                {
                    TeacherDto teacherDto = new TeacherDto()
                    {
                        Username = user.UserName,
                        Surname = user.LastName,
                        Name = user.FirstName,
                        Email = user.Email
                    };
                    return teacherDto;
                }
            }
            return null;      
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
            //if (teacher == null)
            //{
            //    return null;
            //}
            TeacherDto user = db.TeachersRepository.TeachertoTeacherDto(teacher);
            if (user != null && teacher.Subjects != null)
            {
                    foreach (var subject in teacher.Subjects)
                    {
                        SubjectDto subjectDto = db.SubjectsRepository.SubjectToDto(subject.Subject);
                        user.Subjects.Add(subjectDto);
                    } 
            }
            return user;
        }
        public Teacher TeacherDtoToTeacher(TeacherDto teacherDto)
        {
            //if (teacherDto == null)
            //{
            //    return null;
            //}
            Teacher teacher = db.TeachersRepository.TeacherDtoToTeacher(teacherDto);
            TeachesSubjects ts = new TeachesSubjects();
            if (teacherDto.Subjects != null)
            {
                foreach (var subject in teacherDto.Subjects)
                {
                    Subject subjectt = db.SubjectsRepository.DtoToSubject(subject);
                    ts.Teacher = teacher;
                    ts.Subject = subjectt;
                    teacher.Subjects.Add(ts);
                }
            }
            return teacher;
        }

    }
}