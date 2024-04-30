using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Service.Services
{
    public class MailServer
    {
        public static void fillMailInformations()
        {
            string mailstatus = SendEmail("b200109031@subu.edu.tr", "Testmail", "Hey i have setup my own SMTP server.Let us check it out!!!");
            Console.WriteLine(mailstatus);
            Console.ReadKey();
        }
        public static string SendEmail(string toAddress, string subject, string body)
        {
            string result = "Message Sent Successfully..!!";
            string senderID = "admin@kutuphanem.com";
            const string senderPassword = "benadmin";
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
                result = "Error sending email.!!!";
            }
            return result;
        }
    }
}

