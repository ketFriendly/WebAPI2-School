using It_Girls_Projekat.DTOs;
using It_Girls_Projekat.Models;
using It_Girls_Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace It_Girls_Projekat.Repositories
{
    public interface IParentsRepository:IGenericRepository<Parent>
    {
        Parent ChangeEmail(Guid id, string email);
        bool? IsValidEmail(string email);
        //Parent GetByUserName(string username);
        UserDto ParentToUserDto(Parent parent);
        Parent UserDtoToParent(UserDto user);
        ParentDto ParentToParentDto(Parent parent);
        Parent ParentDtoToParent(ParentDto parentDto);
    }
}
