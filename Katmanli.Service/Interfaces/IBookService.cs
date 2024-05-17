using Katmanli.Core.Response;
using Katmanli.DataAccess.DTOs;
using Katmanli.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Katmanli.Service.Interfaces
{
    public interface IBookService
    {
        IResponse<IEnumerable<BookQuery>> ListAll();
        IResponse<IEnumerable<BookQuery>> ListBooksByCategoryId(int categoryId);
        IResponse<IEnumerable<BookQuery>> ListBooksByAuthorId(int authorId);
        IResponse<IEnumerable<BookQuery>> FindById(int id);
        IResponse<string> Update(BookUpdate model);
        IResponse<string> Create(BookCreate model);
        IResponse<string> Delete(int id);

        IResponse<string> askQueryToAIModel(string inputBookString);
        IResponse<string> UpdateIsAvailable(BookUpdate model);
    }
}
