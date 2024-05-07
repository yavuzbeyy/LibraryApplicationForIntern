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

                _parameterList.Add("@Name",  model.Name );
                _parameterList.Add("@Surname", model.Surname );
                _parameterList.Add("@YearOfBirth",  model.YearOfBirth );
                _parameterList.Add("@PlaceOfBirth", model.PlaceOfBirth);
                _parameterList.Add("@FotoKey", model.fotoKey);

                var requestResult = _databaseExecutions.ExecuteQuery("Sp_AuthorCreate", _parameterList);

                // Return success response
                return new SuccessResponse<string>("Yazar başarılı bir şekilde oluşturuldu.");
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
                _parameterList.Reset();

                _parameterList.Add("@DeleteById",id);

                var requestResult = _databaseExecutions.ExecuteDeleteQuery("Sp_AuthorsDeleteById", _parameterList);

                if (requestResult > 0)
                {
                    return new SuccessResponse<string>(Messages.Delete("Yazar"));
                }
                else
                {
                    return new ErrorResponse<string>(Messages.DeleteError("Yazar"));
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
                _parameterList.Reset();
                _parameterList.Add("@Id",id);

                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_AuthorsGetById", _parameterList);

                var selectedAuthor = JsonConvert.DeserializeObject<IEnumerable<AuthorQuery>>(jsonResult);

                if (selectedAuthor.IsNullOrEmpty())
                {
                    //böyle bir yazar bulunamadı.
                    return new ErrorResponse<IEnumerable<AuthorQuery>>(Messages.NotFound("Yazar"));
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

        public IResponse<string> Update(AuthorUpdate model)
        {
            try
            {
                _parameterList.Reset();
                _parameterList.Add("@AuthorId", model.Id);
                _parameterList.Add("@Name", model.Name);
                _parameterList.Add("@Surname", model.Surname);
                _parameterList.Add("@YearOfBirth", model.YearOfBirth);
                _parameterList.Add("@PlaceOfBirth", model.PlaceOfBirth);
                _parameterList.Add("@UpdatedDate", DateTime.Now);

                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_AuthorUpdate", _parameterList);

                return new SuccessResponse<string>(Messages.Update("Yazar"));
            }
            catch (Exception ex)
            {
                return new ErrorResponse<string>(ex.Message);
            }
        }
    }
}
