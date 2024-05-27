using Katmanli.Core.Response;
using Katmanli.Core.SharedLibrary;
using Katmanli.DataAccess.Connection;
using Katmanli.DataAccess.DTOs;
using Katmanli.Service.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Parameter = Katmanli.DataAccess.DTOs.Parameter;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Newtonsoft.Json.Linq;
using Confluent.Kafka;


namespace Katmanli.Service.Services
{
    public class BookService : IBookService
    {
        private readonly ParameterList _parameterList;
        private readonly DatabaseExecutions _databaseExecutions;

        private InferenceSession _session;

        public BookService(ParameterList parameterList,DatabaseExecutions databaseExecutions)
        {
            _parameterList = parameterList;
            _databaseExecutions = databaseExecutions;
            InitializeModel();
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
                _parameterList.Reset();
                _parameterList.Add("@BookId", model.Id);
                 _parameterList.Add("@Title", model.Title);
                 _parameterList.Add("@Description", model.Description);
                 _parameterList.Add("@PublicationYear", model.PublicationYear);
                 _parameterList.Add("@NumberOfPages", model.NumberOfPages);
                 _parameterList.Add("@IsAvailable", model.isAvailable);
                 _parameterList.Add("@AuthorId", model.AuthorId);
                 _parameterList.Add("@CategoryId", model.CategoryId);
                 _parameterList.Add("@UpdatedDate", DateTime.Now);

                    var jsonResult = _databaseExecutions.ExecuteQuery("Sp_BookUpdate", _parameterList);

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

        //Yapay zeka model işlemleri
        private void InitializeModel()
        {
            string modelPath = "C:/Users/yavuz/OneDrive/Desktop/VakifbankStaj/YapayZekaModeli/model.onnx";
            _session = new InferenceSession(modelPath);
        }

        public IResponse<string> askQueryToAIModel(string inputBookString)
        {
            try
            {
                // Türkçe karakterler için Windows1254'e çevirme
              // Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
               // Encoding windows1254 = Encoding.GetEncoding(1254); 
                //byte[] windows1254Bytes = windows1254.GetBytes(inputBookString);
                //string convertedBookContent = windows1254.GetString(windows1254Bytes);

                //byte[] utf8Bytes = Encoding.UTF8.GetBytes(inputBookString);
                //string convertedString = Encoding.UTF8.GetString(utf8Bytes);
                //string convertedBookContent = windows1254.GetString(utf8Bytes);


                //Encoding iso = Encoding.GetEncoding("ISO-8859-9");
                //Encoding utf8 = Encoding.UTF8;
                //byte[] utfBytes = utf8.GetBytes(inputBookString);
                //byte[] w1254Bytes = Encoding.Convert(utf8, iso, utfBytes);
                //string msg = iso.GetString(w1254Bytes);


                var inputs = new List<NamedOnnxValue>
                 {
                  NamedOnnxValue.CreateFromTensor("string_input", new DenseTensor<string>(new[] { inputBookString }, new[] { 1, 1 }))
                 };

                using (var results = _session.Run(inputs))
                {
                    var outputLabel = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<string>();


                    var outputProbabilities = results.FirstOrDefault(item => item.Name == "output_probability").Value as List<Microsoft.ML.OnnxRuntime.DisposableNamedOnnxValue>;


                    var getListOfProbabilities = outputProbabilities[0].Value as System.Collections.Generic.Dictionary<System.String, System.Single>;


                    var labelAndProbabilityFromModel = getListOfProbabilities.OrderByDescending(item => item.Value).First();


                    if (outputLabel == null || outputProbabilities == null)
                    {
                        throw new InvalidOperationException("Model did not return any output.");
                    }

                    //Güven skoru kontrolü yapalım
                    if (labelAndProbabilityFromModel.Value < 0.55) 
                    {
                        return new SuccessResponse<string>($"Düşük güven skoru. Lütfen daha belirgin parametreler giriniz. {labelAndProbabilityFromModel.Value}");
                    }

                    // string responseMessage = $"Predicted category: {labelAndProbabilityFromModel.Key} with probability: {labelAndProbabilityFromModel.Value}";
                    string responseMessage = $"{labelAndProbabilityFromModel.Key}";
                    return new SuccessResponse<string>(responseMessage);
                    }
            }
            catch (Exception ex)
            {
                return new ErrorResponse<string>($"Error predicting book category: {ex.Message}");
            }
        }
    }
}
    

    

