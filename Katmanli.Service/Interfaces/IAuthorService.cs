using Katmanli.Core.Response;
using Katmanli.DataAccess.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Service.Interfaces
{
    public interface IAuthorService
    {
        IResponse<IEnumerable<AuthorQuery>> ListAll();
        IResponse<IEnumerable<AuthorQuery>> FindById(int id);
        IResponse<string> Update(AuthorUpdate model);
        IResponse<string> Create(AuthorCreate model);
        IResponse<string> Delete(int id);
    }
}
