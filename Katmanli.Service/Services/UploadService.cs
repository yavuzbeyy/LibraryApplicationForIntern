using Katmanli.Core.Response;
using Katmanli.DataAccess.Connection;
using Katmanli.DataAccess.DTOs;
using Katmanli.DataAccess.Entities;
using Katmanli.Service.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Katmanli.DataAccess.DTOs.CategoryDTO;

namespace Katmanli.Service.Services
{
    public class UploadService : IUploadService
    {
        private readonly IConfiguration _configuration;
        private readonly ParameterList _parameterList;
        private readonly DatabaseExecutions _databaseExecutions;
        public UploadService(IConfiguration configuration,DatabaseExecutions databaseExecutions,ParameterList parameterList)
        {
            _configuration = configuration;
            _databaseExecutions = databaseExecutions;
            _parameterList = parameterList;
        }

        public string UploadFile(IFormFile file, int bookId)
        {
            if (file != null && file.Length > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                string fileExtension = Path.GetExtension(fileName).ToLower();

                string documentIdentityKey = Guid.NewGuid().ToString();      
                string documentGuidName = documentIdentityKey + fileExtension;
                string fileType = GetFileType(fileName);
                
                //Dosya resim değilse hata döndür. Bunun kontrolünü uide de yapabilirim.
                if (fileType != "Resim") 
                {
                    throw new Exception(""); //hata döndür
                }

                // FTP bağlantı bilgilerini config dosyasından al
                string ftpServer =   _configuration.GetValue<string>("FileUploadService:ConnectionAdress");
                string ftpUsername = _configuration.GetValue<string>("FileUploadService:FTPUsername");
                string ftpPassword = _configuration.GetValue<string>("FileUploadService:Password");


                // Dosyayı doğrudan FTP sunucusuna yükle
                string uploadedFilePath = UploadFileToFtp(file, documentGuidName, ftpServer, ftpUsername, ftpPassword);

                if (!uploadedFilePath.IsNullOrEmpty())
                { 
                _parameterList.Reset();
                _parameterList.Add("@FileOriginalName", fileName);
                _parameterList.Add("@FileGuidedName", documentGuidName);
                _parameterList.Add("@FileKey", documentIdentityKey);
                _parameterList.Add("@FilePath", uploadedFilePath);
                _parameterList.Add("@BookId", bookId);
                var requestResult = _databaseExecutions.ExecuteQuery("Sp_UploadImageCreate", _parameterList);
                }          
            }
            return null; //DataResult dönülücek.
        }


        private string UploadFileToFtp(IFormFile file, string documentGuidName, string ftpServer, string ftpUsername, string ftpPassword)
        {
            try
            {

                string uploadPath = $"//FTPUser/Uploads/Images/{documentGuidName}";

                string fullUploadPath = $"{ftpServer}{uploadPath}";
                Uri uri = new Uri(fullUploadPath);

                // FTP isteği oluşturma
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                using (Stream ftpStream = request.GetRequestStream())
                {
                    file.CopyTo(ftpStream);
                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
                }

                return uploadPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FTP Upload Error: {ex.Message}");
                throw; 
            }
        }


        private string GetFileType(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".jfif":
                    return "Resim";
                default:
                    return "Diger";
            }
        }

        public IFormFile GetFile(int bookId)
        {
            _parameterList.Reset();

            _parameterList.Add("BookId",bookId);

            var jsonResult = _databaseExecutions.ExecuteQuery("Sp_GetFileGuidedNamesByBookId", _parameterList);

            var bookImage = JsonConvert.DeserializeObject<IEnumerable<UploadImage>>(jsonResult).FirstOrDefault();

            if(bookImage != null) 
            { 
            var formFile = getImageFromFtpServer(bookImage.FileKey);

                if(formFile != null) 
                {
                    return formFile;
                }
               
            }
            return null;
        }

        private IFormFile getImageFromFtpServer(string guidedImageName)
        {
            try
            {
                string ftpServer = _configuration.GetValue<string>("FileUploadService:ConnectionAdress");
                string ftpUsername = _configuration.GetValue<string>("FileUploadService:FTPUsername");
                string ftpPassword = _configuration.GetValue<string>("FileUploadService:Password");
                string ftpViewUrl = _configuration.GetValue<string>("FileUploadService:ViewUrl");


                string completedViewUrl = $"{ftpViewUrl}{guidedImageName}";

                byte[] fileBytes;

                // FTP sunucu bağlantı bilgileri
                Uri uri = new Uri(completedViewUrl);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);


                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            responseStream.CopyTo(memoryStream);
                            fileBytes = memoryStream.ToArray();
                        }
                    }
                }
                string contentType = "application/octet-stream";

                var fileContent = new MemoryStream(fileBytes);
                var formFile = new FormFile(fileContent, 0, fileBytes.Length, contentType, guidedImageName);

                return formFile;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FTP Image Download Error: {ex.Message}");
                throw;
            }
        }


    }
}
