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
    [RoutePrefix("api/subjects")]
    public class SubjectsController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ISubjectsService db;
        public SubjectsController(ISubjectsService db)
        {
            this.db = db;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            IEnumerable<Subject> subjects = db.Get();
            return Ok(subjects);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet,Route("all")]
        public IHttpActionResult GetAllSubjects()
        {
            IEnumerable<SubjectDto> dtos = db.GetAllSubjects();
            return Ok(dtos);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpGet,Route("{id:int}")]
        public IHttpActionResult GetSubject (int id)
        {
            SubjectDto subject = db.GetSubject(id);
            if (subject == null)
            {
                return NotFound();
            }
            return Ok(subject);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost, Route("")]
        public IHttpActionResult PostSubject(SubjectDto subject)
        {
            if (ModelState.IsValid!=true)
            {
                return BadRequest(ModelState);
            }
            string result= "";
            try
            {
                result = db.AddASubject(subject);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return BadRequest();
            }
             
            return Ok(result);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPut, Route("{id:int}")]
        public IHttpActionResult PutSubject(int id, SubjectDto subject)
        {
            if (ModelState.IsValid!=true)
            {
                return BadRequest(ModelState);
            }
            string result = "";
            try
            {
                result = db.UpdateSubject(id, subject);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut, Route("{sub_id:int}/add-existing-teacher/{tea_id}")]
        public IHttpActionResult AddTeacherToSubject(int sub_id, Guid tea_id)
        {
            string result = "";
            try
            {
                result = db.AddTeacherToSubject(sub_id, tea_id);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return BadRequest();
            }
            if (result.Contains("successfully"))
            {
                return Ok(result);
            }
            return NotFound();
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpDelete, Route("{id:int}")]
        public IHttpActionResult DeleteSubject(int id)
        {
            try
            {
                SubjectDto subject = db.DeleteSubject(id);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            return Ok(id);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpGet, Route("{idSubject:int}/subjectsClasses/{idTeacher}")]
        public IHttpActionResult GetSubjectsClasses(Guid idTeacher, int idSubject)
        {
            List<Class> classes = db.GetClassBySubject(idTeacher, idSubject);
            if (classes == null)
            {
                return NotFound();
            }
            return Ok(classes);

        }



    }
}
