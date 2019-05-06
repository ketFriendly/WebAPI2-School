using It_Girls_Projekat.DTOs;
using It_Girls_Projekat.Models;
using It_Girls_Projekat.Repositories;
using It_Girls_Projekat.Services;
using Microsoft.AspNet.Identity;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace It_Girls_Projekat.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IUsersService service;

        public AccountController(IUsersService userService)
        {
            this.service = userService;
        }

        //[Authorize(Roles ="Admin")]
        //[Route("all")]
        //public IEnumerable<ApplicationUser> GetApplicationUsers()
        //{
        //    IEnumerable<ApplicationUser> users = service.GetAllUsers();
        //    return users;
        //}
        [Authorize(Roles = "Admin")]
        [Route("register-student")]
        public async Task<IHttpActionResult> RegisterStudent(UserDto userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityResult result = await service.RegisterStudent(userModel);
            IHttpActionResult errorResult = GetErrorResult(result);
            if (result == null)
            {
                return BadRequest("There is an user with that username");
            }
            if (errorResult != null)
            {
                return errorResult;
            }
            return Ok();
        }
    
        [Authorize(Roles = "Admin")]
        [Route("register-students")]
        public async Task<IHttpActionResult> RegisterStudents(IEnumerable<UserDto> userModels)
        {
            
            if (userModels == null)
            {
                return BadRequest();
            }
            var results = await service.RegisterStudents(userModels);
           
            if (results == null)
            {
                return null;
            }
            return Ok();

        }

        [Authorize(Roles = "Admin")]
        [Route("register-parent")]
        public async Task<IHttpActionResult> RegisterParent(UserDto userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityResult result = await service.RegisterParent(userModel);
            IHttpActionResult errorResult = GetErrorResult(result);
            if (errorResult != null)
            {
                return errorResult;
            }
            return Ok();
        }   

        [Authorize(Roles = "Admin")]
        [Route("register-teacher")]
        public async Task<IHttpActionResult> RegisterTeacher(UserDto userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityResult result = await service.RegisterTeacher(userModel);
            if (result == null)
            {
                return BadRequest("There is an user with that username");
            }
            IHttpActionResult errorResult = GetErrorResult(result);
            if (errorResult != null)
            {
                return errorResult;
            }
            return Ok();
        }

        [AllowAnonymous]
        [Route("register-admin")]
        public async Task<IHttpActionResult> RegisterAdmin(UserDto userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityResult result = await service.RegisterAdmin(userModel);
            IHttpActionResult errorResult = GetErrorResult(result);
            if (errorResult != null)
            {
                return errorResult;
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            
            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }
            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }
                return BadRequest(ModelState);
            }
            return null;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet,Route("logs")]
        public IHttpActionResult GetLogs()
        {
            String path = @"C:\Users\KORISNIK\source\repos\It_Girls_Projekat\It_Girls_Projekat\logs\app-log.txt";
            List<string> logs = new List<string>();
            using (StreamReader sr = File.OpenText(path))
            {
                String s = "";

                while ((s = sr.ReadLine()) != null)
                {
                    logs.Add(s);
                }
            }
            return Ok(logs);
        }
       
    }
}

