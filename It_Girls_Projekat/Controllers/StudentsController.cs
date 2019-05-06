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
    [RoutePrefix("api/students")]
    public class StudentsController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IStudentsService db;
        public StudentsController(IStudentsService db)
        {
            this.db = db;
        }
        [Authorize(Roles = "Admin, Teacher")]
        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            IEnumerable<Student> students = db.GetAll();
            if (students == null)
            {
                return NotFound();
            }
            return Ok(students);
        }
        [Authorize(Roles = "Admin, Teacher")]
        [HttpGet,Route("all")]
        public IHttpActionResult GetAllStudents()
        {
            IEnumerable<StudentDto> students = db.GetAllStudents();
            if (students == null)
            {
                return NotFound();
            }
            return Ok(students);
        }

        [Authorize(Roles = "Admin, Teacher, Parent, Student")]
        [HttpGet,Route("{id}")]
        public IHttpActionResult GetStudent(Guid id)
        {
            StudentDto student = db.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut, Route("{id}")]
        public IHttpActionResult PutStudent(Guid id, UserDto student)
        {
            
            if (ModelState.IsValid != true)
            {
                return BadRequest(ModelState);
            }
            try
            {
                StudentDto user = db.GetStudent(id);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            return Ok(db.UpdateStudent(id, student.FirstName, student.LastName, student.UserName,student.Password));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete, Route("delete/{id}")]
        public IHttpActionResult DeleteStudent(Guid id)
        {
            UserDto student = new UserDto();
            try
            {
               student = db.DeleteStudent(id);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            return Ok(student);
        }

        [Authorize(Roles = "Admin, Student")]
        [HttpPut, Route("change_email/{id}")]
        public IHttpActionResult PutEmail([FromUri]Guid id, [FromBody]string email)
        {
            StudentDto student = new StudentDto();
            try
            {
                student = db.ChangeEmail(id, email);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            return Ok(student);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut, Route("{id}/existing-parent/{idParent}")]
        public IHttpActionResult PutExistingParent(Guid id, Guid idParent)
        {
            string result = db.AddExistingParent(idParent, id);
            if (result.Contains("successfully"))
            {
                return Ok(result);
            }
            return Content(HttpStatusCode.NotFound, result); ;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut, Route("create-parent/{id}")]
        public IHttpActionResult PutNewParent([FromUri]Guid id, [FromBody]UserDto parent)
        {
            if (ModelState.IsValid != true)
            {
                return BadRequest(ModelState);
            }
            string result = db.AddNewParent(id, parent, parent.Password);
            if (result.Contains("successfully"))
            {
                return Ok(result);
            }
            return Content(HttpStatusCode.NotFound, result);
        }

        [Authorize(Roles = "Admin, Parent")]
        [HttpGet, Route("getbyparent/{id}")]
        public IHttpActionResult GetByParent([FromUri]Guid id)
        {
            List<Student> children = new List<Student>();
            try
            {
                children = db.GetByParent(id);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            return Ok(children);
        }

        //[HttpPost, Route("create-student-parent")]
        //public IHttpActionResult PostBulkParentStudent(UserDto user_parent, string pass, UserDto user_student, string stud_pass)
        //{
        //    if (ModelState.IsValid != true)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    string result = db.AddBulkParentStudent(user_parent, pass, user_student, stud_pass);
        //    if (result.Contains("successfully"))
        //    {
        //        return Ok(result);
        //    }
        //    return Content(HttpStatusCode.NotFound, result);
        //}

        //[HttpGet, Route("by-username")]
        //public IHttpActionResult GetByUserName(string username)
        //{
        //    UserDto student = db.GetByUserName(username);
        //    if (student == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(student);
        //}
    }
}
