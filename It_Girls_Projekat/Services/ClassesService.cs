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
    public class ClassesService:IClassesService
    {
        private IUnitOfWork db;

        public ClassesService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }
        public IEnumerable<Class> Get()
        {
            IEnumerable<Class> classes = db.ClassesRepository.Get();
           
            if (classes != null)
            {
                return classes;
            }
            return null;
        }
        public IEnumerable<ClassDto> GetAllClasses()
        {
            IEnumerable<Class> classes = db.ClassesRepository.Get();
            List<ClassDto> classDtos = new List<ClassDto>();
            if (classes != null)
            {
                foreach (var clas in classes)
                {
                    ClassDto dto = ClassToDto(clas);
                    if (dto != null)
                    {
                        classDtos.Add(dto);
                    }
                }
                return classDtos;
            }
            return null;
        }
        public ClassDto GetClass(int id)
        {
            Class classs = db.ClassesRepository.GetByID(id);
            if (classs == null)
            {
                return null;
            }
            ClassDto dto = ClassToDto(classs);
            return dto;
        }
        public ClassDto CreateAClass(ClassDto classs)
        {
            if (classs == null)
            {
                return null;
            }
            Class cl = DtoToClass(classs);
            db.ClassesRepository.Insert(cl);
            db.Save();
            return classs;
        }
        public string AddStudentToClass(int id, Guid student_id)
        {
            Class cl = db.ClassesRepository.GetByID(id);
            Student st = db.StudentsRepository.GetByID(student_id.ToString());
            if (cl == null || st == null)
            {
                return null;
            }

            cl.Students.Add(st);
            st.Class = cl;
            db.ClassesRepository.Update(cl);
            db.StudentsRepository.Update(st);
            db.Save();
            return "Student added successfully";
        }
        public string UpdateClass (int id, Class classForUpdate)
        {
            Class cl = db.ClassesRepository.GetByID(id);
            if (cl == null)
            {
                return null;
            }
            //IEnumerable<Student> studs = classForUpdate.Students;
            //foreach (var student in cl.Students)
            //{
            //    if (true)
            //    {

            //    }
            //}
            //{
            //    Student s = db.StudentsRepository.UserDtoToStudent(student);
            //    studs.Add(s);
            //}
            cl.Grade = classForUpdate.Grade;
            cl.ClassNo = classForUpdate.ClassNo;

            //if (classForUpdate.Students.Count != 0)
            //{

            //}
            //cl.Students = studs;
            db.ClassesRepository.Update(cl);
            db.Save();
            return "Class updated successfully";
        }
        public string DeleteClass (int id)
        {
            Class cl = db.ClassesRepository.GetByID(id);
            
            if (cl == null)
            {
                return "There is no class with that id";
            }
            if (cl.Students.Count != 0)
            {
                foreach (var student in cl.Students)
                {
                    student.IdClass = null;
                    student.Class = null;
                    db.StudentsRepository.Update(student);
                }
            }
            List<TeachesSubjects> tsListEmp = cl.TeachesSubjects.ToList();
            List<int> idTeachSubs = new List<int>();
            if (tsListEmp.Count!=0)
            {
                foreach (TeachesSubjects item in tsListEmp)
                {
                    while(item.Classes.Count != 0)
                    {
                        if (item.Classes.Remove(cl))
                        {
                            db.teach_subj_repo.Update(item);
                        }
                    }
                   
                }
            }
            

            db.ClassesRepository.Delete(cl);
            db.Save();
            return "Class deleted successfully";
        }
        public ClassDto ClassToClassDto(Class classs)
        {
            ClassDto dto = new ClassDto();
            foreach (var item in classs.Students)
            {
                dto.Students.Add(db.StudentsRepository.StudentToStudentDto(item));
            }
            dto = db.ClassesRepository.ClassToClassDto(classs, dto.Students);
            return dto;
        }
        public Class ClassDtoToClass (ClassDto classDto)
        {
            Class classs = new Class();
            foreach (var item in classDto.Students)
            {
                classs.Students.Add(db.StudentsRepository.StudentDtoToStudent(item));
            }
            classs = db.ClassesRepository.ClassDtoToClass(classDto, classs.Students.ToList());
            return classs;
        }
        public ClassDto ClassToDto(Class classs)
        {
            ClassDto dto = new ClassDto()
            {
                ClassNo = classs.ClassNo,
                Grade = classs.Grade
            };
            if (classs.Students.Count != 0)
            {
                foreach (var student in classs.Students)
                {
                    StudentDto st = db.StudentsRepository.StudentToStudentDto(student);
                    dto.Students.Add(st);
                }
            }
            return dto;
        }
        public Class DtoToClass(ClassDto classDto)
        {
            Class classs = new Class()
            {
                ClassNo = classDto.ClassNo,
                Grade = classDto.Grade
            };
            if (classDto.Students != null)
            {
                foreach (var student in classDto.Students)
                {
                    classs.Students.Add(db.StudentsRepository.StudentDtoToStudent(student));
                }
            }
            return classs;
        }
        public List<Student> GetStudents(int id)
        {
            IEnumerable<Student> allStudents = db.StudentsRepository.Get();
            List<Student> studentsInClass = new List<Student>();
            foreach (var item in allStudents)
            {
                if (item.IdClass == id)
                {
                    studentsInClass.Add(item);
                }
            }
            if (studentsInClass.Count == 0)
            {
                return null;
            }
            return studentsInClass;
        }
        public List<Student> GetStudentsWithoutClass()
        {
            IEnumerable<Student> students = db.StudentsRepository.Get();

            if (students == null)
            {
                return null;
            }
            List<Student> studentsCl = new List<Student>();
            foreach (var student in students)
            {
                if (student.Class == null)
                {
                    studentsCl.Add(student);
                }
            }
            return studentsCl;
        }
        public IEnumerable<Subject> GetSubjectsWithoutClass()
        {
            IEnumerable<Subject> subjects = db.SubjectsRepository.Get();
            if (subjects == null)
            {
                return null;
            }
          
            List<Subject> subs = subjects.ToList();
            
            List<Subject> subjTeach = new List<Subject>();
            IEnumerable<Subject> subjClTeach = new List<Subject>();
            List<TeachesSubjects> teachesSubjects = new List<TeachesSubjects>();
            
            for (int i = 0; i < subs.Count; i++)
            {
                if (subs[i].Teachers.Count != 0)
                {
                    foreach (var teSu in subs[i].Teachers)
                    {
                        if (teSu.Classes.Count == 0)
                        {
                            subjTeach.Add(subs[i]);
                        }
                    }
                }
            }
            //foreach (var item in collection)
            //{

            //}
            //IEnumerable<TeachesSubjects> ts = db.teach_subj_repo.Get();
            //List<int> idS = new List<int>();
            ////idSubject without class list
            //foreach (var item in ts)
            //{
            //    if (item.Classes.Count == 0)
            //    {
            //        idS.Add(item.IdSubject);
            //    }
            //}
            //List<int> result = idS.Distinct().ToList();
            //foreach (int res in result)
            //{
            //    subjClTeach = subjTeach.Where(x => x.IdSubject == res);
            //}
            return subjTeach;
        }
        public List<Teacher> GetTeachers(int id)
        {
            IEnumerable<TeachesSubjects> teach = db.teach_subj_repo.Get();
            List<TeachesSubjects> teaches = teach.ToList();
            List<Teacher> teachers = new List<Teacher>();
            if (teach != null)
            {
                for (int i = 0; i < teaches.Count; i++)
                {
                    if (teaches[i].Classes != null)
                    {
                        foreach (var cl in teaches[i].Classes)
                        {
                            if (cl.IdClass == id)
                            {
                                teachers.Add(teaches[i].Teacher);
                            }
                        }

                    }
                }
                return teachers;
            }
            return null;
        }
        public Class AddSubject (int idClass, int idSubject)
        {
            Class classs = db.ClassesRepository.GetByID(idClass);
            Subject subject = db.SubjectsRepository.GetByID(idSubject);
            
            IEnumerable<TeachesSubjects> teachSubs = db.teach_subj_repo.Get();
            List<TeachesSubjects> teacSu = new List<TeachesSubjects>();
            if (subject.Teachers != null)
            {
                foreach (var ts in subject.Teachers)
                {
                    if (ts.IdSubject == idSubject)
                    {
                        classs.TeachesSubjects.Add(ts);
                        ts.Classes.Add(classs);
                    }
                    
                }
            }
           
            //foreach (var item in teachSubs)
            //{
            //    if (item.IdSubject == idSubject)
            //    {
            //        //u teaches subject dodajemo class
            //        item.Classes.Add(classs);
            //        teacSu.Add(item);
            //        db.teach_subj_repo.Update(item);
            //    }
            //}

            //subject.Teachers = teacSu;
            db.SubjectsRepository.Update(subject);
            db.ClassesRepository.Update(classs);
            db.Save();
            return classs;
        }
    }
}