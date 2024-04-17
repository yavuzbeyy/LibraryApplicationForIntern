using Katmanli.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Katmanli.DataAccess.DTOs.CategoryDTO;

namespace Katmanli.Service.Interfaces
{
    public interface ICategoryService
    {
        IResponse<IEnumerable<CategoryQuery>> ListAll();
        IResponse<IEnumerable<CategoryQuery>> FindById(int id);
        IResponse<string> Update(CategoryUpdate model);
        IResponse<string> Create(CategoryCreate model);
        IResponse<string> Delete(int id);
    }
}
