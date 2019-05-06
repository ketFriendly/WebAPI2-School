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
    [RoutePrefix("api/teachers")]
    public class TeachersController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ITeachersService db;
        public TeachersController(ITeachersService db)
        {
            this.db = db;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet, Route("")]
        public IHttpActionResult GetAll()
        {
            IEnumerable<Teacher> teachers;
            try
            {
                teachers = db.Get();
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            logger.Info("Requesting teachers");
            return Ok(teachers);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet,Route("all")]
        public IHttpActionResult Get()
        {
            IEnumerable<TeacherDto> teachers;
            
            try
            {
                teachers = db.GetAllTeachers();
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            logger.Info("Requesting usernames");
            
            return Ok(teachers);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpGet, Route("getAllTeachersSubjects/{id}")]
        public IHttpActionResult GetAllSubjects(Guid id)
        {
            List<SubjectDto> subjects = db.GetSubjects(id);
            if (subjects == null)
            {
                return NotFound();
            }
            return Ok(subjects);

        }
        [Authorize(Roles = "Admin, Teacher")]
        [HttpGet,Route("{id}")]
        public IHttpActionResult GetTeacher([FromUri]Guid id)
        {
            TeacherDto teacher = db.GetTeacher(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return Ok(teacher);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut,Route("{id}")]
        public IHttpActionResult PutTeacher([FromUri]Guid id, [FromBody]UserDto teacher)
        {
            if (ModelState.IsValid != true)
            {
                return BadRequest(ModelState);
            }
            if (db.GetTeacher(id) == null)
            {
                return NotFound();
            }
            return Ok(db.UpdateTeacher(id, teacher.FirstName, teacher.LastName, teacher.UserName, teacher.Password, teacher.Email));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete, Route("delete/{id}")]
        public IHttpActionResult DeleteTeacher(Guid id)
        {
            TeacherDto teacher = new TeacherDto();
            try
            {
                teacher = db.DeleteTeacher(id);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            return Ok(teacher);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPut, Route("change_email/{id}")]
        public IHttpActionResult PutEmail([FromUri]Guid id, [FromBody]string email)
        {
            TeacherDto teacher = db.ChangeEmail(id, email);
            try
            {
                teacher = db.ChangeEmail(id, email);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            return Ok(teacher);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet,Route("by-username/{username}")]
        public IHttpActionResult GetByUserName([FromUri]string username)
        {
            TeacherDto teacher = new TeacherDto();
            try
            {
                teacher = db.GetByUserName(username);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            
            return Ok(teacher);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpGet, Route("teachersClasses/{id}")]
        public IHttpActionResult GetTeachersClasses (Guid id)
        {
            List<Class> classes = db.GetTeachersClasses(id);
            if (classes == null)
            {
                return NotFound();   
            }
            return Ok(classes);
            
        }


    }
}
