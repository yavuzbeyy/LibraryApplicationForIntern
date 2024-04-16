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
using static Katmanli.DataAccess.DTOs.CategoryDTO;

namespace Katmanli.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ParameterList _parameterList;
        private readonly DatabaseExecutions _databaseExecutions;

        public CategoryService(ParameterList parameterList, DatabaseExecutions databaseExecutions)
        {
            _parameterList = parameterList;
            _databaseExecutions = databaseExecutions;
        }

        public IResponse<string> Create(CategoryCreate model)
        {
            try {
            // Reset parameter list
            _parameterList.Reset();

           _parameterList.Add(new Parameter { Name = "@Name", Value = model.Name });

            var requestResult = _databaseExecutions.ExecuteQuery("Sp_CategoryCreate", _parameterList);

            // Return success response
            return new SuccessResponse<string>("Category created successfully.");
        }
            catch (Exception ex)
            {
                return new ErrorResponse<string>($"Failed to create category: {ex.Message}");
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

                var requestResult = _databaseExecutions.ExecuteDeleteQuery("Sp_CategoriesDeleteById", _parameterList);

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

        public IResponse<IEnumerable<CategoryQuery>> FindById(int id)
        {
            try
            {
                Parameter parameter = new Parameter();

                parameter.Name = "@Id";
                parameter.Value = id;
                _parameterList.Add(parameter);

                var jsonResult = _databaseExecutions.ExecuteQuery("Sp_CategoriesGetById", _parameterList);

                var selectedCategory = JsonConvert.DeserializeObject<IEnumerable<CategoryQuery>>(jsonResult);

                if (selectedCategory.IsNullOrEmpty())
                {
                    //böyle bir kitap bulunamadı.
                    return new ErrorResponse<IEnumerable<CategoryQuery>>(Messages.NotFound("Kitap"));
                }

                return new SuccessResponse<IEnumerable<CategoryQuery>>(selectedCategory);
            }
            catch (Exception ex)
            {
                return new ErrorResponse<IEnumerable<CategoryQuery>>(ex.Message);
            }
        }

        public IResponse<IEnumerable<CategoryQuery>> ListAll()
        {

                try
                {
                    _parameterList.Reset();

                    var jsonResult = _databaseExecutions.ExecuteQuery("Sp_CategoriesGetAll", _parameterList);

                    var categories = JsonConvert.DeserializeObject<List<CategoryQuery>>(jsonResult);

                    return new SuccessResponse<IEnumerable<CategoryQuery>>(categories);
                }
                catch (Exception ex)
                {
                    return new ErrorResponse<IEnumerable<CategoryQuery>>(ex.Message);
                }
     
        }

        public IResponse<CategoryDTO.CategoryQuery> Update(CategoryDTO.CategoryUpdate model)
        {
            throw new NotImplementedException();
        }
    }
}
