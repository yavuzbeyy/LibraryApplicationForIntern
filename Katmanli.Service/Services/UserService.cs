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
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ITokenCreator _tokenCreator;
        private readonly DatabaseExecutions _databaseExecutions;
        private readonly SpParameters _spInformation;

        public UserService(IMapper mapper, ITokenCreator tokenCreator,DatabaseExecutions databaseExecutions, SpParameters parameters)
        {
            _mapper = mapper;
            _tokenCreator = tokenCreator;
            _databaseExecutions = databaseExecutions;
            _spInformation = parameters;
        }

        public IResponse<string> Create(UserCreate model)
        {
            try
            {
                _databaseExecutions.UserAddQuery("Sp_UserCreate",model);
                return new SuccessResponse<string>(Messages.Save("User"));
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
                _spInformation.Reset();
                _spInformation.DeleteById = id;

                var requestResult = _databaseExecutions.UserExecuteQuery("Sp_UsersDeleteById", _spInformation);

                if (int.Parse(requestResult) > 0) 
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
                _spInformation.Reset();
                _spInformation.FindById = id;

                var jsonResult = _databaseExecutions.UserExecuteQuery("Sp_UsersGetById",_spInformation);

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

        public IResponse<IEnumerable<UserQuery>> GetUserByUsername(string username)
        {
            try
            {
                _spInformation.Reset();
                _spInformation.FindByName = username;

                var jsonResult = _databaseExecutions.UserExecuteQuery("Sp_UsersGetByUsername",_spInformation);

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
                _spInformation.Reset();
                var jsonResult = _databaseExecutions.UserExecuteQuery("Sp_UsersGetAll", _spInformation);

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

        public async Task<IResponse<UserQuery>> Update(UserUpdate model)
        {
            throw new NotImplementedException();
        }
    }
}
