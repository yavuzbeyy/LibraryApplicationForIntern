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

        //// Delegate tanımlaması
        //public delegate void Notify(string message);

        //public Notify delegateornek;

        //static kullanıyordum Zipkin eklemek için mailServer instance oluşturdum
        private readonly IMailServer _mailServer;

        public UserService(ITokenCreator tokenCreator, DatabaseExecutions databaseExecutions, ParameterList parameterList, IMailServer mailServer)
        {
            _tokenCreator = tokenCreator;
            _databaseExecutions = databaseExecutions;
            _parameterList = parameterList;
            _mailServer = mailServer;
        }

        public IResponse<string> Create(UserCreate model)
        {
            try
            {
                //Hashlenmiş Password
                string hashedPassword = _tokenCreator.GenerateHashedPassword(model.Password);

                _parameterList.Reset();

                _parameterList.Add("@Name", model.Name);
                _parameterList.Add("@Surname", model.Surname);
                _parameterList.Add("@Username", model.Username);
                _parameterList.Add("@Email", model.Email);
                _parameterList.Add("@UpdatedDate", DateTime.Now);
                _parameterList.Add("@Password", hashedPassword);
                _parameterList.Add("@RoleId", model.RoleId);

                string result = _databaseExecutions.ExecuteQuery("Sp_UserCreate", _parameterList);

                return new SuccessResponse<string>("Kullanıcı başarılı bir şekilde kayıt edildi.");
            }
            catch (Exception ex)
            {
                return new ErrorResponse<string>($"Failed to create user: {ex.Message}");
            }
        }

        public IResponse<string> CreateBookRequest(BookRequestCreateDto model)
        {
            try
            {
                var parameterList = new ParameterList();

                parameterList.Add("@RequestDate", model.CreatedDate);
                parameterList.Add("@BookId", model.BookId);
                parameterList.Add("@UserId", model.UserId);
                parameterList.Add("@ReturnDate", model.ReturnDate);
                parameterList.Add("@isApproved", false);
                //değerlendirildi mi eklenecek

                string result = _databaseExecutions.ExecuteQuery("Sp_CreateBookRequest", parameterList);

                return new SuccessResponse<string>("Kitap isteği başarılı bir şekilde oluşturuldu.");
            }
            catch (Exception ex)
            {
                return new ErrorResponse<string>($"Failed to create book request: {ex.Message}");
            }
        }

        public IResponse<string> Delete(int id)
        {
            try
            {

                _parameterList.Reset();

                _parameterList.Add("@DeleteById", id);

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

        public IResponse<string> DeleteBookRequest(int id)
        {
            try
            {
                _parameterList.Reset();
                _parameterList.Add("@DeleteById", id);

                var requestResult = _databaseExecutions.ExecuteDeleteQuery("Sp_RequestDeleteById", _parameterList);

                //if (requestResult > 0 )
                //{}
                return new SuccessResponse<string>("Kitap isteği başarıyla silindi");

                //else
                //{
                //    return new ErrorResponse<string>("Belirtilen id ile eşleşen bir kitap isteği bulunamadı");
                //}
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
                _parameterList.Add("@Id", id);

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

        public IResponse<IEnumerable<BookRequestQuery>> GetAllRequests()
        {
            try
            {
                _parameterList.Reset();

                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_BookRequestsGetAll", _parameterList);

                var bookRequests = JsonConvert.DeserializeObject<List<BookRequestQuery>>(jsonResult);

                return new SuccessResponse<IEnumerable<BookRequestQuery>>(bookRequests);
            }
            catch (Exception ex)
            {
                return new ErrorResponse<IEnumerable<BookRequestQuery>>(ex.Message);
            }
        }

        public IResponse<IEnumerable<UserQuery>> GetUserByUsername(string username)
        {
            try
            {
                _parameterList.Reset();
                _parameterList.Add("@Username", username);

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
            string hashedPassword = _tokenCreator.GenerateHashedPassword(loginModel.Password);

            _parameterList.Reset();
            _parameterList.Add("@Username", loginModel.Username);
            _parameterList.Add("@Password", hashedPassword);

            var jsonResult = _databaseExecutions.ExecuteQuery("Sp_UserLogin", _parameterList);

            var loginUser = JsonConvert.DeserializeObject<List<UserQuery>>(jsonResult);

            if (loginUser != null && loginUser.Any())
            {
                var roles = loginUser.Select(u => u.RoleId).ToList();
                string token = _tokenCreator.GenerateToken(loginModel.Username,loginUser.First().Id, roles);

                return new SuccessResponse<string>(token);
            }
            return new ErrorResponse<string>(Messages.NotFound("Token"));
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

        public IResponse<string> PasswordReminder(string username)
        {
            try
            {
                var parameterList = new ParameterList();
                parameterList.Add("@Username", username);
                

                string generatedNewPassword = newPasswordGenerate();

                string newPasswordHashkey = _tokenCreator.GenerateHashedPassword(generatedNewPassword);
                parameterList.Add("@NewPassword", newPasswordHashkey);

                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_RemindPassword", parameterList);

                var userInformation = JsonConvert.DeserializeObject<List<UserCreate>>(jsonResult).First();

                _mailServer.fillMailInformations(userInformation.Email, generatedNewPassword,userInformation.Username);

                return new SuccessResponse<string>("Şifreniz mailinize gönderildi.");
            }
            catch (Exception ex)
            {
                return new ErrorResponse<string>(ex.Message);
            }
        }

        private string newPasswordGenerate() 
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_@-.,?!";

            Random random = new Random();
            int passwordLength = random.Next(4, 11); 

            char[] password = new char[passwordLength];
            for (int i = 0; i < passwordLength; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }

            return new string(password);
        }

        public IResponse<string> CreateGroupforMessages(string groupName)
        {
            try
            {
                var parameterList = new ParameterList();

                parameterList.Add("@Name", groupName);

                string result = _databaseExecutions.ExecuteQuery("Sp_CreateGroupforMessages", parameterList);

                return new SuccessResponse<string>("Grup başarılı bir şekilde oluşturuldu.");
            }
            catch (Exception ex)
            {
                return new ErrorResponse<string>($"Failed to create group: {ex.Message}");
            }
        }

        public IResponse<IEnumerable<UserGroupsWithUsersDTO>> ListAllMessageGroups()
        {
            try
            {
                _parameterList.Reset();

                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_GroupsGetAllByDetails", _parameterList);

                var groups = JsonConvert.DeserializeObject<List<UserGroupsWithUsersDTO>>(jsonResult);

                return new SuccessResponse<IEnumerable<UserGroupsWithUsersDTO>>(groups);
            }
            catch (Exception ex)
            {
                return new ErrorResponse<IEnumerable<UserGroupsWithUsersDTO>>(ex.Message);
            }
        }

        public IResponse<string> DeleteGroupById(int groupId)
        {
            try
            {

                _parameterList.Reset();

                _parameterList.Add("@GroupId", groupId);

                var requestResult = _databaseExecutions.ExecuteDeleteQuery("Sp_GroupsDeleteById", _parameterList);

                if (requestResult > 0)
                {
                    return new SuccessResponse<string>(Messages.Delete("Grup"));
                }
                else
                {
                    return new ErrorResponse<string>(Messages.DeleteError("Grup"));
                }

            }
            catch (Exception ex)
            {
                return new ErrorResponse<string>(ex.Message);
            }
        }

        public IResponse<string> addUserToGroup(string username,int groupId)
        {
            try
            {
                var parameterList = new ParameterList();

                parameterList.Add("@Username", username);
                parameterList.Add("@GroupId", groupId);

                string result = _databaseExecutions.ExecuteQuery("Sp_AddUserToGroup", parameterList);

                return new SuccessResponse<string>("Grup başarılı bir şekilde oluşturuldu.");
            }
            catch (Exception ex)
            {
                return new ErrorResponse<string>($"Failed to create group: {ex.Message}");
            }
        }
    }
}
