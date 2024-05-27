using Katmanli.DataAccess.DTOs;
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
        string ExecuteQuery(string storedProcedureName, ParameterList parameters);
        int ExecuteDeleteQuery(string storedProcedureName, ParameterList parameters);
    }
}
