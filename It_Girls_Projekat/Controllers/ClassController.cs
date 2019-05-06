using It_Girls_Projekat.DTOs;
using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using It_Girls_Projekat.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace It_Girls_Projekat.Controllers
{
    [RoutePrefix("api/classes")]
    public class ClassController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IClassesService db;
        public ClassController(IClassesService db)
        {
            this.db = db;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            IEnumerable<Class> classs = db.Get();
            if (classs == null)
            {
                return NotFound();
            }
            return Ok(classs);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet,Route("all")]
        public IHttpActionResult GetAllClasses()
        {
            IEnumerable<ClassDto> dtos = db.GetAllClasses();
            if (dtos == null)
            {
                return NotFound();
            }
            return Ok(dtos);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet,Route("{id:int}")]
        public IHttpActionResult GetClass(int id)
        {
            ClassDto dto = db.GetClass(id);
            if (dto == null)
            {
                return NotFound();
            }
            return Ok(dto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, Route("")]
        public IHttpActionResult PostClass(ClassDto classs)
        {
            if (ModelState.IsValid!= true)
            {
                return BadRequest(ModelState);
            }
            ClassDto cl = new ClassDto();
            try
            {
                cl = db.CreateAClass(classs);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return BadRequest();
            }
            return Ok(cl);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut, Route("{id:int}/add-student/{student_id}")]
        public IHttpActionResult PutStudentToClass([FromUri]int id, [FromUri]Guid student_id)
        {
            string result = "";
            try
            {
                result = db.AddStudentToClass(id, student_id);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut, Route("{id:int}")]
        public IHttpActionResult PutClass([FromUri]int id,  Class classForUpdate)
        {
            string result = "";
            try
            {
                result = db.UpdateClass(id, classForUpdate);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound(); 
            }
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete, Route("{id:int}")]
        public IHttpActionResult DeleteClass(int id)
        {
            string result = "";
            try
            {
                result = db.DeleteClass(id);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet, Route("studentsInClass/{id:int}")]
        public IHttpActionResult GetAllStudents(int id)
        {
            List<Student> students = db.GetStudents(id);
            if (students == null)
            {
                return NotFound();
            }
            return Ok(students);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet, Route("studentsNoClass")]
        public IHttpActionResult GetAllStudentsWithoutClass()
        {
            List<Student> students = db.GetStudentsWithoutClass();
            if (students == null)
            {
                return NotFound();
            }
            return Ok(students);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet, Route("subjectsNoClass")]
        public IHttpActionResult GetAllSubjectsWithoutClass()
        {
            IEnumerable<Subject> subjects = db.GetSubjectsWithoutClass();
            if (subjects == null)
            {
                return NotFound();
            }
            return Ok(subjects);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet, Route("teachesClass/{id:int}")]
        public IHttpActionResult GetTeachers( int id)
        {
            List<Teacher> teachers = db.GetTeachers(id);
            if (teachers == null)
            {
                return NotFound();
            }
            return Ok(teachers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut, Route("{idClass:int}/addSubject/{idSubject:int}")]
        public IHttpActionResult PutSubject (int idClass, int idSubject)
        {
            Class classs = db.AddSubject(idClass, idSubject);
            if (classs == null)
            {
                return null;
            }
            return Ok(classs);
        }
    }
}
