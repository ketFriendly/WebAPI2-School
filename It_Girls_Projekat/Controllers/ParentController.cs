using It_Girls_Projekat.DTOs;
using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using It_Girls_Projekat.Repositories;
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
    [RoutePrefix("api/parents")]
    public class ParentController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IParentsService db;
        public ParentController(IParentsService db)
        {
            this.db = db;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            IEnumerable<Parent> parents;
            try
            {
                parents = db.Get();
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            logger.Info("Requesting parents");
            return Ok(parents);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet,Route("all")]
        public IHttpActionResult GetAllParents()
        {
            IEnumerable<UserDto> parents ;
            try
            {
                parents = db.GetAllParents();
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            logger.Info("Requesting parents");
            return Ok(parents);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet,Route("{id}")]
        public IHttpActionResult GetParent(Guid id)
        {
            ParentDto parent = new ParentDto();
            try
            {
                parent = db.GetParent(id);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            return Ok(parent);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut, Route("{id}")]
        
        public IHttpActionResult PutParent([FromUri]Guid id, [FromBody]UserDto parent)
        {
            if (ModelState.IsValid != true)
            {
                return BadRequest(ModelState);
            }
            ParentDto parentDto = new ParentDto();
            try
            {
                parentDto = db.GetParent(id);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            return Ok(db.UpdateParent(id, parent.FirstName, parent.LastName, parent.UserName, parent.Email, parent.Password));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete, Route("delete/{id}")]
        public IHttpActionResult DeleteParent(Guid id)
        {
            ParentDto parent;
            try
            {
                parent = db.DeleteParent(id);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            logger.Info("Deleted a parent" + parent.Name + " " + parent.Surname);
            return Ok(parent);
        }

        [Authorize(Roles = "Admin, Parent")]
        [HttpPut, Route("change_email/{id}")]
        public IHttpActionResult PutEmail([FromUri]Guid id, [FromBody]string email)
        {
            ParentDto parent =new ParentDto();
            try
            {
                parent = db.ChangeEmail(id, email);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            
            return Ok(parent);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet,Route("by-username/{username}")]
        public IHttpActionResult GetByUserName([FromUri]string username)
        {
            ParentDto parent = new ParentDto();
            try
            {
                parent = db.GetByUserName(username);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            logger.Info("Search by username");
            return Ok(parent);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut, Route("{id}/existing-student/{idStudent}")]
        public IHttpActionResult PutExistingStudent(Guid id, Guid idStudent)
        {
            string result = db.AddExistingStudent(id, idStudent);
            if (result.Contains("successfully"))
            {
                return Ok(result);
            }
            return Content(HttpStatusCode.NotFound, result); ;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut, Route("create-student/{id}")]
        public IHttpActionResult PutNewStudent(Guid id, UserDto student)
        {
            if (ModelState.IsValid != true)
            {
                return BadRequest(ModelState);
            }
            string result = db.AddNewStudent(id, student, student.Password);
            if (result.Contains("successfully"))
            {
                return Ok(result);
            }
            return Content(HttpStatusCode.NotFound, result);

        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost, Route("create-student-parent")]
        public IHttpActionResult PostBulkParentStudent (UserDto user_parent, UserDto user_student)
        {
            if (ModelState.IsValid != true)
            {
                return BadRequest(ModelState);
            }
            string result = db.AddBulkParentStudent(user_parent, user_parent.Password, user_student, user_student.Password);
            if (result.Contains("successfully"))
            {
                return Ok(result);
            }
            return Content(HttpStatusCode.NotFound, result);
        }
    }
}
