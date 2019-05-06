using It_Girls_Projekat.Infrastructure;
using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace It_Girls_Projekat.Repositories
{
    public class ClassesRepository:GenericRepository<Class>, IClassesRepository
    {
        private AuthContext db;
        public ClassesRepository(AuthContext db):base(db)
        {
            this.db = db;
        }
        public ClassDto ClassToDto(Class classs)
        {
            ClassDto dto = new ClassDto()
            {
                ClassNo = classs.ClassNo,
                Grade = classs.Grade
            };
            if (dto == null)
            {
                return null;
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
            if (classs == null)
            {
                return null;
            }
            return classs;
        }
        public ClassDto ClassToClassDto(Class classs, List<StudentDto>students)
        {
            ClassDto dto = new ClassDto()
            {
                ClassNo = classs.ClassNo,
                Grade = classs.Grade,
                Students = students
            };
            if (dto == null)
            {
                return null;
            }
            return dto;
        }
        public Class ClassDtoToClass(ClassDto classDto, List<Student> students)
        {
            Class classs = new Class()
            {
                ClassNo = classDto.ClassNo,
                Grade = classDto.Grade,
                Students = students
            };
            if (classs == null)
            {
                return null;
            }
            return classs;
        }
    }
}