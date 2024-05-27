using Katmanli.DataAccess.DTOs;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Service.Services
{
   
    public interface IMailServer 
    {
        Task fillMailInformations(string mailAdress, string password, string username);
        string SendEmail(string toAddress, string subject, string body);
    }

    public class MailServer : IMailServer
    {

        private readonly Tracer _tracer;

        private readonly int a;

        //Bir mantığı yok aslında zipkinde birbirine bağımlı apileri görmek için çalıştırdım.
        private readonly HttpClient _httpClient;

        public MailServer(TracerProvider tracerProvider, HttpClient httpClient)
        {
            _tracer = tracerProvider.GetTracer("MailServer");
            _httpClient = httpClient;
        }

        public async Task fillMailInformations(string mailAdress,string password,string username)
        {
            string apiResponse = await GetApiResponseAsync("http://localhost:5097/api/Soru/List");
            string mailstatus = SendEmail(mailAdress, $"Şifre Sıfırlama Servisi", $"Şifrenizi unuttuğunuz için üzgünüz ama endişelenmeyin sizin için yeniledik :) Şifreniz: {password} , Kullanıcı Adınız :{username}");
        }

        private async Task<string> GetApiResponseAsync(string apiUrl)
        {
            using (var span = _tracer.StartActiveSpan("GetApiResponse"))
            {
                try
                {
                    HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    span.RecordException(ex);
                    return $"Error fetching API response: {ex.Message}";
                }
            }
        }
        public string SendEmail(string toAddress, string subject, string body)
        {
            string result = "Message Sent Successfully..!!";
            string senderID = "admin@kutuphanem.com";
            const string senderPassword = "benadmin";

            //Zipkin yapılandırması bu Trace kaldırılabilir.
            using (var span = _tracer.StartActiveSpan("SendEmail"))
            {
                try
                {
                SmtpClient smtp = new SmtpClient
                {
                    Host = "localhost",
                    Port = 25,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 30000,
                };
                MailMessage message = new MailMessage(senderID, toAddress, subject, body);
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!!" + ex;
            }
            return result;
            }
        }
    }
}


