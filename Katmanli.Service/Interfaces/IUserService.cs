using Katmanli.Core.Response;
using Katmanli.DataAccess.DTOs;
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
        IResponse<UserQuery> FindById(int id);
        Task<IResponse<UserQuery>> Update(UserUpdate model);
        Task<IResponse<string>> Create(UserCreate model);
        Task<IResponse<string>> Delete(int id);
        IResponse<string> Login(UserLoginDto loginModel);
        IResponse<UserQuery> GetUserByUsername(string username);
    }
}
