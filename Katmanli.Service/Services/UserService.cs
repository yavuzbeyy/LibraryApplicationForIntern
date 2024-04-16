using AutoMapper;
using Katmanli.Core.Interfaces.DataAccessInterfaces;
using Katmanli.Core.Interfaces.ServiceInterfaces;
using Katmanli.Core.Response;
using Katmanli.Core.SharedLibrary;
using Katmanli.DataAccess.Connection;
using Katmanli.DataAccess.DTOs;
using Katmanli.DataAccess.Entities;
using Katmanli.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Service.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenCreator _tokenCreator;
        private readonly DatabaseExecutions _databaseExecutions;
        private readonly ParameterList _parameterList;

        public UserService(ITokenCreator tokenCreator, DatabaseExecutions databaseExecutions, ParameterList parameterList)
        {
            _tokenCreator = tokenCreator;
            _databaseExecutions = databaseExecutions;
            _parameterList = parameterList;
        }

        public IResponse<string> Create(UserCreate model)
        {
            try
            {
                //Hashlenmiş Password
                string hashedPassword = _tokenCreator.GenerateHashedPassword(model.Password);

                // Reset parameter list
                var parameterList = new ParameterList();

                parameterList.Add(new Parameter { Name = "@Name", Value = model.Name });
                parameterList.Add(new Parameter { Name = "@Surname", Value = model.Surname });
                parameterList.Add(new Parameter { Name = "@Username", Value = model.Username });
                parameterList.Add(new Parameter { Name = "@Email", Value = model.Email });
                parameterList.Add(new Parameter { Name = "@UpdatedDate", Value = DateTime.Now });
                parameterList.Add(new Parameter { Name = "@Password", Value = hashedPassword });
                parameterList.Add(new Parameter { Name = "@RoleId", Value = model.RoleId });

                string result = _databaseExecutions.ExecuteQuery("Sp_UserCreate", parameterList);

                // Return success response
                return new SuccessResponse<string>("User created successfully.");
            }
            catch (Exception ex)
            {
                return new ErrorResponse<string>($"Failed to create user: {ex.Message}");
            }
        }



        public IResponse<string> Delete(int id)
        {
            try
            {
                Parameter parameter = new Parameter();
                _parameterList.Reset();

                parameter.addParameter("@DeleteById", id);
                _parameterList.Add(parameter);

                var requestResult = _databaseExecutions.ExecuteDeleteQuery("Sp_UsersDeleteById", _parameterList);

                if (requestResult > 0)
                {
                    return new SuccessResponse<string>(Messages.Delete("Kullanıcı"));
                }
                else
                {
                    return new ErrorResponse<string>(Messages.DeleteError("Kullanıcı"));
                }

            }
            catch (Exception ex)
            {
                return new ErrorResponse<string>(ex.Message);
            }
        }

        public IResponse<IEnumerable<UserQuery>> FindById(int id)
        {
            try
            {
                Parameter parameter = new Parameter();

                parameter.addParameter("@Id", id);

                _parameterList.Add(parameter);

                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_UsersGetById", _parameterList);

                var users = JsonConvert.DeserializeObject<IEnumerable<UserQuery>>(jsonResult);

                if (users.IsNullOrEmpty())
                {
                    return new ErrorResponse<IEnumerable<UserQuery>>(Messages.NotFound("Kullanıcı"));
                }

                return new SuccessResponse<IEnumerable<UserQuery>>(users);
            }
            catch (Exception ex)
            {
                return new ErrorResponse<IEnumerable<UserQuery>>(ex.Message);
            }

        }

        public IResponse<IEnumerable<UserQuery>> GetUserByUsername(string username)
        {
            try
            {
                Parameter parameter = new Parameter();
                parameter.addParameter("@Username", username);

                _parameterList.Add(parameter);

                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_UsersGetByUsername", _parameterList);

                var users = JsonConvert.DeserializeObject<IEnumerable<UserQuery>>(jsonResult);

                if (users.IsNullOrEmpty())
                {
                    //böyle bir kullanıcı bulunamadı.
                    return new ErrorResponse<IEnumerable<UserQuery>>(Messages.NotFound("Kullanıcı"));
                }

                return new SuccessResponse<IEnumerable<UserQuery>>(users);
            }
            catch (Exception ex)
            {
                return new ErrorResponse<IEnumerable<UserQuery>>(ex.Message);
            }
        }

        public IResponse<IEnumerable<UserQuery>> ListAll()
        {
            try
            {
                _parameterList.Reset();

                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_UsersGetAll", _parameterList);

                var users = JsonConvert.DeserializeObject<List<UserQuery>>(jsonResult);

                return new SuccessResponse<IEnumerable<UserQuery>>(users);
            }
            catch (Exception ex)
            {
                return new ErrorResponse<IEnumerable<UserQuery>>(ex.Message);
            }
        }

        public IResponse<string> Login(UserLoginDto loginModel)
        {
            //var girisYapanKullanici = _userRepository.Queryable().Where(x => x.Username == loginModel.Username).FirstOrDefault();

            //if (girisYapanKullanici != null)
            //{
            //    var roles = _userRoleRepository.GetAll().Where(x => x.UserId == girisYapanKullanici.Id).Select(x => x.RoleId).ToList();

            //    string token = _tokenCreator.GenerateToken(loginModel.Username, roles);

            //    return new SuccessResponse<string>(token);
            //}

            //return new ErrorResponse<string>(Messages.NotFound("Token"));

            return null;
        }

        public IResponse<UserQuery> Update(UserUpdate model)
        {
            throw new NotImplementedException();
        }

    }
}
