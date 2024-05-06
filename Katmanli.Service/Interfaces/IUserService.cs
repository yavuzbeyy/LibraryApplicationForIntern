using Katmanli.Core.Response;
using Katmanli.DataAccess.DTOs;
using Katmanli.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Core.Interfaces.ServiceInterfaces
{
    public interface IUserService
    {
        IResponse<IEnumerable<UserQuery>> ListAll();
        IResponse<IEnumerable<UserQuery>> FindById(int id);
        IResponse<string> Update(UserUpdate model);
        IResponse<string> Create(UserCreate model);
        IResponse<string> CreateBookRequest(BookRequestCreateDto model);
        IResponse<string> Delete(int id);
        IResponse<string> DeleteBookRequest(int id);
        IResponse<string> Login(UserLoginDto loginModel);
        IResponse<IEnumerable<UserQuery>> GetUserByUsername(string username);
        IResponse<IEnumerable<BookRequestQuery>> GetAllRequests();
        IResponse<string> PasswordReminder(string username);
        IResponse<string> CreateGroupforMessages(string groupName);
        IResponse<IEnumerable<UserGroupsWithUsersDTO>> ListAllMessageGroups();
        IResponse<string> DeleteGroupById(int groupId);
        IResponse<string> addUserToGroup(string username, int groupId);
    }
}
