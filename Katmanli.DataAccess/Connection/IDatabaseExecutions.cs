using Katmanli.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.DataAccess.Connection
{
    public interface IDatabaseExecutions
    {
        List<Role> ExecuteStoredProcedure(string storedProcedureName);
    }
}
