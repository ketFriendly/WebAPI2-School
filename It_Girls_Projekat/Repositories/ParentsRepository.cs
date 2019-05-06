using It_Girls_Projekat.DTOs;
using It_Girls_Projekat.Infrastructure;
using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace It_Girls_Projekat.Repositories
{
    public class ParentsRepository: GenericRepository<Parent>, IParentsRepository
    {
        //private AuthRepository repo;
        private AuthContext db;
        public ParentsRepository(AuthContext db):base (db)
        {
            this.db = db;
        }
        public Parent ChangeEmail(Guid id, string email)
        {
            Parent parent = GetByID(id.ToString());
            if (parent == null || email == null)
            {
                return null;
            }
            bool? valid = IsValidEmail(email);
            if (valid == false || valid == null)
            {
                return null;
            }
            parent.Email = email;
            return parent;
        }
        public bool? IsValidEmail(string email)
        {
            //try
            //{
            //    var addr = new System.Net.Mail.MailAddress(email);
            //    return addr.Address == email;
            //}
            //catch
            //{
            //    return false;
            //}
            if (email == null)
            {
                return null;
            }
            bool valid = email.EndsWith(".com") && email.Contains("@");
            return valid;
        }

        public UserDto ParentToUserDto(Parent parent)
        {
            if (parent == null)
            {
                return null;
            }
            UserDto user = new UserDto()
            {
                UserName = parent.UserName,
                FirstName = parent.FirstName,
                LastName = parent.LastName,
                Email = parent.Email
            };
            return user;
        }
        public Parent UserDtoToParent(UserDto user)
        {
            if (user == null)
            {
                return null;
            }
            Parent parent = new Parent()
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email, 
                PasswordHash = user.Password
            };
            return parent;
        }
        public ParentDto ParentToParentDto(Parent parent)
        {
            if (parent == null)
            {
                return null;
            }
            ParentDto user = new ParentDto();
            
            user.Username = parent.UserName;
            user.Name = parent.FirstName;
            user.Surname = parent.LastName;
            user.Email = parent.Email;
            return user;
        }
        public Parent ParentDtoToParent(ParentDto parentDto)
        {
            if (parentDto == null)
            {
                return null;
            }
            Parent parent = new Parent();
            parent.UserName = parentDto.Username;
            parent.FirstName = parentDto.Name;
            parent.LastName = parentDto.Surname;
            parent.Email = parentDto.Email;
            
            return parent;
        }
    }
}