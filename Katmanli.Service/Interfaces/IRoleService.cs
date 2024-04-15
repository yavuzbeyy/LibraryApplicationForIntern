using Katmanli.Core.Response;
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
    }
}
