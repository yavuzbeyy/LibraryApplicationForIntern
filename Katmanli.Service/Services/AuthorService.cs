using Katmanli.Core.Response;
using Katmanli.Core.SharedLibrary;
using Katmanli.DataAccess.Connection;
using Katmanli.DataAccess.DTOs;
using Katmanli.Service.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Katmanli.Service.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly ParameterList _parameterList;
        private readonly DatabaseExecutions _databaseExecutions;
        
        public AuthorService(ParameterList parameterList, DatabaseExecutions databaseExecutions) 
        {
            _databaseExecutions = databaseExecutions;
            _parameterList = parameterList;
        }
        public IResponse<string> Create(AuthorCreate model)
        {
            try
            {
                // Reset parameter list
                _parameterList.Reset();

                _parameterList.Add(new Parameter { Name = "@Name", Value = model.Name });
                _parameterList.Add(new Parameter { Name = "@Surname", Value = model.Surname });
                _parameterList.Add(new Parameter { Name = "@YearOfBirth", Value = model.YearOfBirth });

                var requestResult = _databaseExecutions.ExecuteQuery("Sp_AuthorCreate", _parameterList);

                // Return success response
                return new SuccessResponse<string>("Author created successfully.");
            }
            catch (Exception ex)
            {
                return new ErrorResponse<string>($"Failed to create Author: {ex.Message}");
            }
        }
    

        public IResponse<string> Delete(int id)
        {
            try
            {
                Parameter parameter = new Parameter();
                _parameterList.Reset();

                parameter.Name = "@DeleteById";
                parameter.Value = id;
                _parameterList.Add(parameter);

                var requestResult = _databaseExecutions.ExecuteDeleteQuery("Sp_AuthorsDeleteById", _parameterList);

                if (requestResult > 0)
                {
                    return new SuccessResponse<string>(Messages.Delete("Kategori"));
                }
                else
                {
                    return new ErrorResponse<string>(Messages.DeleteError("Kategori"));
                }

            }
            catch (Exception ex)
            {
                return new ErrorResponse<string>(ex.Message);
            }
        }

        public IResponse<IEnumerable<AuthorQuery>> FindById(int id)
        {
            try
            {
                Parameter parameter = new Parameter();

                parameter.Name = "@Id";
                parameter.Value = id;
                _parameterList.Add(parameter);

                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_AuthorsGetById", _parameterList);

                var selectedAuthor = JsonConvert.DeserializeObject<IEnumerable<AuthorQuery>>(jsonResult);

                if (selectedAuthor.IsNullOrEmpty())
                {
                    //böyle bir kitap bulunamadı.
                    return new ErrorResponse<IEnumerable<AuthorQuery>>(Messages.NotFound("Kitap"));
                }

                return new SuccessResponse<IEnumerable<AuthorQuery>>(selectedAuthor);
            }
            catch (Exception ex)
            {
                return new ErrorResponse<IEnumerable<AuthorQuery>>(ex.Message);
            }
        }

        public IResponse<IEnumerable<AuthorQuery>> ListAll()
        {
            try
            {
                _parameterList.Reset();

                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_AuthorsGetAll", _parameterList);

                var categories = JsonConvert.DeserializeObject<List<AuthorQuery>>(jsonResult);

                return new SuccessResponse<IEnumerable<AuthorQuery>>(categories);
            }
            catch (Exception ex)
            {
                return new ErrorResponse<IEnumerable<AuthorQuery>>(ex.Message);
            }
        }

        public IResponse<AuthorQuery> Update(AuthorUpdate model)
        {
            throw new NotImplementedException();
        }
    }
}
