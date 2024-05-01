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

                _parameterList.Add("@Title", model.Title );
                _parameterList.Add("@PublicationYear", model.PublicationYear);
                _parameterList.Add("@Description", model.Description);
                _parameterList.Add("@NumberOfPages" ,model.NumberOfPages);
                _parameterList.Add("@IsAvailable", model.isAvailable);
                _parameterList.Add("@AuthorId",model.AuthorId);
                _parameterList.Add("@CategoryId",model.CategoryId);
                _parameterList.Add("@FileKey", model.Filekey);


                var requestResult = _databaseExecutions.ExecuteQuery("Sp_BookCreate", _parameterList);

                return new SuccessResponse<string>("Kitap başarılı bir şekilde oluşturuldu.");
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

                _parameterList.Add("@DeleteById", id);

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

                _parameterList.Add("@Id",id);

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

                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_BooksGetAllWithAuthors", _parameterList);

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

                _parameterList.Add("@CategoryId", categoryId);

                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_BooksGetByCategoryId", _parameterList);

                var books = JsonConvert.DeserializeObject<List<BookQuery>>(jsonResult);

                return new SuccessResponse<IEnumerable<BookQuery>>(books);
            }
            catch (Exception ex)
            {
                return new ErrorResponse<IEnumerable<BookQuery>>(ex.Message);
            }
        }

        public IResponse<IEnumerable<BookQuery>> ListBooksByAuthorId(int authorId)
        {
            try
            {
                _parameterList.Reset();

                _parameterList.Add("@AuthorId", authorId);

                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_BooksGetByAuthorId", _parameterList);

                var books = JsonConvert.DeserializeObject<List<BookQuery>>(jsonResult);

                return new SuccessResponse<IEnumerable<BookQuery>>(books);
            }
            catch (Exception ex)
            {
                return new ErrorResponse<IEnumerable<BookQuery>>(ex.Message);
            }
        }

        public IResponse<string> Update(BookUpdate model)
        {
                try
                {
                    var parameterList = new ParameterList();
                    parameterList.Add("@BookId", model.Id);
                    parameterList.Add("@Title", model.Title);
                    parameterList.Add("@Description", model.Description);
                     parameterList.Add("@PublicationYear", model.PublicationYear);
                    parameterList.Add("@NumberOfPages", model.NumberOfPages);
                    parameterList.Add("@IsAvailable", model.isAvailable);
                    parameterList.Add("@AuthorId", model.AuthorId);
                    parameterList.Add("@CategoryId", model.CategoryId);
                    parameterList.Add("@UpdatedDate", DateTime.Now);

                    var jsonResult = _databaseExecutions.ExecuteQuery("Sp_BookUpdate", parameterList);

                    return new SuccessResponse<string>(Messages.Update("Book"));
                }
                catch (Exception ex)
                {
                    return new ErrorResponse<string>(ex.Message);
                }
            }

        public IResponse<string> UpdateIsAvailable(BookUpdate model)
        {
            try
            {
                var parameterList = new ParameterList();
                parameterList.Add("@BookId", model.Id);
                parameterList.Add("@IsAvailable", model.isAvailable);
  
                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_BookUpdateIsAvailable", parameterList);

                return new SuccessResponse<string>(Messages.Update("Book"));
            }
            catch (Exception ex)
            {
                return new ErrorResponse<string>(ex.Message);
            }
        }

    }
    }

