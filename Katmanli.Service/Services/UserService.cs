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

                var parameterList = new ParameterList();

                parameterList.Add("@Name",  model.Name);
                parameterList.Add( "@Surname", model.Surname );
                parameterList.Add("@Username", model.Username );
                parameterList.Add("@Email",  model.Email );
                parameterList.Add("@UpdatedDate", DateTime.Now );
                parameterList.Add("@Password", hashedPassword);
                parameterList.Add( "@RoleId", model.RoleId );

                string result = _databaseExecutions.ExecuteQuery("Sp_UserCreate", parameterList);

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
 
                _parameterList.Reset();

                _parameterList.Add("@DeleteById",id);

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
                _parameterList.Add("@Id",id);

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
                _parameterList.Reset();
                _parameterList.Add("@Username",username);

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

        public IResponse<string> Update(UserUpdate model)
        {
            try
            {
                var parameterList = new ParameterList();
                parameterList.Add("@UserId", model.UserId);
                parameterList.Add("@Name", model.Name);
                parameterList.Add("@Surname", model.Surname);
                parameterList.Add("@Email", model.Email);
                parameterList.Add("@RoleId", model.RoleId);

                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_UsersUpdate", parameterList);

                return new SuccessResponse<string>(Messages.Update("User"));
            }
            catch (Exception ex)
            {
                return new ErrorResponse<string>(ex.Message);
            }
        }

    }
}
