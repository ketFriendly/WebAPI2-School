using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using It_Girls_Projekat.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace It_Girls_Projekat.Controllers
{
    [RoutePrefix("api/marks")]
    public class MarksController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IMarksService db;
        public MarksController(IMarksService db)
        {
            this.db = db;
        }

        [Authorize(Roles = "Student, Admin")]
        [HttpGet, Route("student/{id}")]
        public IHttpActionResult GetStudentMarks(Guid id)
        {
            IEnumerable<MarkDto> Marks;
            try
            {
                Marks = db.GetStudentMarks(id);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            return Ok(Marks.Select(x =>
            {
                x.AccessType = EAccessType.Student;
                return x;
            }));
        }

        [Authorize(Roles = "Parent, Admin")]
        [HttpGet, Route("parent/{id}")]
        public IHttpActionResult GetStudentsMarks(Guid id)
        {
            IEnumerable<MarkDto> marks;
            try
            {
                marks = db.GetStudentsMarks(id);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            return Ok(marks.Select(x =>
            {
                x.AccessType = EAccessType.Parent;
                return x;
            }));
        }

        [Authorize(Roles = "Teacher, Admin")]
        [HttpGet, Route("all/{teacher_id}")]
        public IHttpActionResult GetAllMarks(Guid teacher_id)
        {
            IEnumerable<MarkDto> marks;
            try
            {
                marks = db.GetAllMarks(teacher_id);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            return Ok(marks.Select(x =>
            {
                x.AccessType = EAccessType.Teacher;
                return x;
            }));
        }

        [Authorize(Roles = "Teacher, Admin")]
        [Route("{teacher_id}/{student_id}/{subject_id:int}/{mark:int}")]
        public IHttpActionResult PostMark(string teacher_id, string student_id, int subject_id, int mark)
        {
            try
            {
                Mark markk = db.PostMark(teacher_id, student_id, subject_id, mark);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return BadRequest();
            }
            return Ok();
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpGet, Route("by-subject")]
        public IHttpActionResult GetMarksBySubject([FromBody]string subject)
        {
            List<MarkDto> dtos = new List<MarkDto>();
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            try
            {
                dtos = db.GetMarksBySubject(subject, userId);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
            }
            return Ok(dtos.Select(x =>
            {
                x.AccessType = EAccessType.Teacher;
                return x;
            }));

        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpGet, Route("{subject}/average/{stud_id}")]
        public IHttpActionResult GetAverageMark([FromUri] Guid stud_id,[FromUri]string subject)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            if (userId == null)
            {
                return BadRequest();
            }
            double? avg = new double();
            Guid guid = Guid.Parse(userId);
            try
            {
                avg = db.GetAverageGrade(subject, stud_id, guid);
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message);
                return NotFound();
               
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return BadRequest();
            }
            logger.Info("Calculate average mark");
            return Ok(avg);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost, Route("submit")]
        public IHttpActionResult PostMarks(List<SubmitDto> list)
        {
            List<Mark> marks = new List<Mark>();
            if (list == null)
            {
                return null;
            }
            foreach (var item in list)
            {
                Mark mark = db.PostMark(item.teacherId, item.studentId, item.subjectId, item.mark);
                if (mark != null)
                {
                    marks.Add(mark);
                }
            }
            return Ok(marks);
        }
    }
}
