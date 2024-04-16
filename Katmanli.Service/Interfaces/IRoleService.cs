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
    public interface IRoleService
    {
        IResponse<IEnumerable<Role>> ListAll();
        IResponse<IEnumerable<UserRole>> ListRolesByUser();
        IResponse<string> Create(RoleCreate model);
        IResponse<string> Delete(int id);
    }
}
