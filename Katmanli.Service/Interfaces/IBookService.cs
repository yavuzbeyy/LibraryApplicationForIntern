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
        IResponse<IEnumerable<BookQuery>> FindById(int id);
        IResponse<BookQuery> Update(BookUpdate model);
        IResponse<string> Create(BookCreate model);
        IResponse<string> Delete(int id);
    }
}
