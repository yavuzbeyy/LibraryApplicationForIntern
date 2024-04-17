using Katmanli.Core.Interfaces.ServiceInterfaces;
using Katmanli.Core.Response;
using Katmanli.Core.SharedLibrary;
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

        public IResponse<string> Create(RoleCreate model)
        {
            try { 
            _parameterList.Reset();
            _parameterList.Add("@Name", model.RoleName);

            var requestResult = _databaseExecutions.ExecuteQuery("Sp_RoleCreate", _parameterList);

            return new SuccessResponse<string>("Role created successfully.");
              }       
            catch (Exception ex)
            {
                return new ErrorResponse<string>($"Failed to create book: {ex.Message}");
            }
}

        public IResponse<string> Delete(int id)
        {
            try
            {
                _parameterList.Reset();

                _parameterList.Add("@DeleteById",id);

                var requestResult = _databaseExecutions.ExecuteDeleteQuery("Sp_RolesDeleteById", _parameterList);

                if (requestResult > 0) 
                {
                    return new SuccessResponse<string>(Messages.Delete("Kitap"));
                }
                else
                {
                    return new ErrorResponse<string>(Messages.DeleteError("Kitap"));
                }

            }
            catch (Exception ex)
            {
                return new ErrorResponse<string>(ex.Message);
            }
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

        public IResponse<IEnumerable<UserRole>> ListRolesByUser()
        {
            try
            {
                _parameterList.Reset();

                var jsonResult = _databaseExecutions.ExecuteQuery("ListUserRolesByUsers", _parameterList);

                var users = JsonConvert.DeserializeObject<List<UserRole>>(jsonResult);

                return new SuccessResponse<IEnumerable<UserRole>>(users);
            }
            catch (Exception ex)
            {
                return new ErrorResponse<IEnumerable<UserRole>>(ex.Message);
            }
        }
    }
}
