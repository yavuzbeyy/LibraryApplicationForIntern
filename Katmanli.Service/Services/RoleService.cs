using Katmanli.Core.Interfaces.ServiceInterfaces;
using Katmanli.Core.Response;
using Katmanli.DataAccess.Connection;
using Katmanli.DataAccess.DTOs;
using Katmanli.DataAccess.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Service.Services
{
    public class RoleService : IRoleService
    {
        private readonly DatabaseExecutions _databaseExecutions;
        private readonly ParameterList _parameterList;
        public RoleService(DatabaseExecutions databaseExecutions,ParameterList parameterList) 
        {
            _databaseExecutions = databaseExecutions;
            _parameterList = parameterList;
        }
        public IResponse<IEnumerable<Role>> ListAll()
        {
            try
            {
                _parameterList.Reset();

                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_RolesGetAll", _parameterList);

                var users = JsonConvert.DeserializeObject<List<Role>>(jsonResult);

                return new SuccessResponse<IEnumerable<Role>>(users);
            }
            catch (Exception ex)
            {
                return new ErrorResponse<IEnumerable<Role>>(ex.Message);
            }
        }
    }
}
