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
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Parameter = Katmanli.DataAccess.DTOs.Parameter;

namespace Katmanli.Service.Services
{
    public class BookService : IBookService
    {
        private readonly ParameterList _parameterList;
        private readonly DatabaseExecutions _databaseExecutions;

        public BookService(ParameterList parameterList,DatabaseExecutions databaseExecutions)
        {
            _parameterList = parameterList;
            _databaseExecutions = databaseExecutions;
        }

        public IResponse<string> Create(BookCreate model)
        {
            try
            {
                // Reset parameter list
                _parameterList.Reset();

                // Add parameters for the stored procedure
                _parameterList.Add(new Parameter { Name = "@Title", Value = model.Title });
                _parameterList.Add(new Parameter { Name = "@PublicationYear", Value = model.PublicationYear });
                _parameterList.Add(new Parameter { Name = "@NumberOfPages", Value = model.NumberOfPages });
                _parameterList.Add(new Parameter { Name = "@IsAvailable", Value = model.isAvailable });
                _parameterList.Add(new Parameter { Name = "@AuthorId", Value = model.AuthorId });
                _parameterList.Add(new Parameter { Name = "@CategoryId", Value = model.CategoryId });

                // Execute the stored procedure with the parameter list
                var requestResult = _databaseExecutions.ExecuteQuery("Sp_BookCreate", _parameterList);

                // Return success response
                return new SuccessResponse<string>("Book created successfully.");
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
                Parameter parameter = new Parameter();
                _parameterList.Reset();

                parameter.Name = "@DeleteById";
                parameter.Value = id;
                _parameterList.Add(parameter);

                var requestResult = _databaseExecutions.ExecuteDeleteQuery("Sp_BooksDeleteById", _parameterList);

                if (requestResult != null) //>0
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

        public IResponse<IEnumerable<BookQuery>> FindById(int id)
        {
            try
            {
                Parameter parameter = new Parameter();

                parameter.Name = "@Id";
                parameter.Value = id;
                _parameterList.Add(parameter);

                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_BooksGetById", _parameterList);

                var selectedBook = JsonConvert.DeserializeObject<IEnumerable<BookQuery>>(jsonResult);

                if (selectedBook.IsNullOrEmpty())
                {
                    //böyle bir kitap bulunamadı.
                    return new ErrorResponse<IEnumerable<BookQuery>>(Messages.NotFound("Kitap"));
                }

                return new SuccessResponse<IEnumerable<BookQuery>>(selectedBook);
            }
            catch (Exception ex)
            {
                return new ErrorResponse<IEnumerable<BookQuery>>(ex.Message);
            }
        }

        public IResponse<IEnumerable<BookQuery>> ListAll()
        {
            try
            {
                _parameterList.Reset();

                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_BooksGetAll", _parameterList);

                var kitaplar = JsonConvert.DeserializeObject<List<BookQuery>>(jsonResult);

                return new SuccessResponse<IEnumerable<BookQuery>>(kitaplar);
            }
            catch (Exception ex)
            {
                return new ErrorResponse<IEnumerable<BookQuery>>(ex.Message);
            }
        }

        public IResponse<IEnumerable<BookQuery>> ListBooksByCategoryId(int categoryId)
        {
            try
            {
                _parameterList.Reset();
                Parameter parameter = new Parameter();
                parameter.Name = "@CategoryId";
                parameter.Value = categoryId;

                _parameterList.Add(parameter);

                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_BooksGetByCategoryId", _parameterList);

                var books = JsonConvert.DeserializeObject<List<BookQuery>>(jsonResult);

                return new SuccessResponse<IEnumerable<BookQuery>>(books);
            }
            catch (Exception ex)
            {
                return new ErrorResponse<IEnumerable<BookQuery>>(ex.Message);
            }
        }

        public IResponse<BookQuery> Update(BookUpdate model)
        {
            throw new NotImplementedException();
        }
    }
}
