using Katmanli.Core.Response;
using Katmanli.DataAccess.Connection;
using Katmanli.DataAccess.DTOs;
using Katmanli.DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Katmanli.Service.Services
{
    public class MainHub : Hub
    {

        private readonly ParameterList _parameterList;
        private readonly DatabaseExecutions _databaseExecutions;

        public MainHub(ParameterList parameterList, DatabaseExecutions databaseExecutions)
        {
            _parameterList = parameterList;
            _databaseExecutions = databaseExecutions;
        }
        public async Task BroadcastMessageToAllClient(string message) 
        {
            await Clients.Caller.SendAsync("ReceiveMesasgesForAllClients",message);
        }

        public async Task sendWelcomeMessage()
        {
            await Clients.Caller.SendAsync("WelcomeMessage", "Kütüphanem.com'a hoş geldiniz. Kitap siparişi oluşturabilmek için lütfen kayıt olunuz.");
        }

        public async Task SendChatMessage(string message,string username)
        {
            await Clients.Caller.SendAsync("MessageSentFromClient", username + " Kullanicisi Şu Mesajı Gönderdi: " + message);
        }

        public async Task SendChatMessageClientOnly(string message, string username,string isAdminSent,string? adminSentToHim,bool? isGroupMessage)
        {
            //kullanıcının mesajı Admine İlet
            if(isAdminSent == "2")
            {
                await Clients.All.SendAsync("ShowMessageToAdmin", message ,  username);
            }
            //Adminin mesajını kullanıcıya ilet
            if (isAdminSent == "1")
            {
                await Clients.All.SendAsync("ShowAdminMessageToUser", message);
            }

            addMessageToDatabase(message,username,isAdminSent, adminSentToHim, isGroupMessage);
        }

        public async Task GetPreviousMessages(string username)
        {
            // Reset parameter list
            _parameterList.Reset();
            _parameterList.Add("@Username", username);
            var dbResult = _databaseExecutions.ExecuteQuery("Sp_GetUserMessages", _parameterList);

            // Deserialize the database result to a list of messages
            var messages = JsonConvert.DeserializeObject<List<UserMessages>>(dbResult);

            // Send the messages to the client
            await Clients.Caller.SendAsync("ShowPreviousMessages", messages);
        }

        public async Task GetPreviousMessagesFromGroups(string groupname)
        {
            // Reset parameter list
            _parameterList.Reset();
            _parameterList.Add("@Groupname", groupname);
            var dbResult = _databaseExecutions.ExecuteQuery("Sp_GetUserMessagesByGroupname", _parameterList);

            // Deserialize the database result to a list of messages
            var messages = JsonConvert.DeserializeObject<List<UserMessages>>(dbResult);

            // Send the messages to the client
            await Clients.Caller.SendAsync("ShowPreviousMessages", messages, "username");
        }


        private void addMessageToDatabase(string message, string username,string isAdminSent,string? adminSentToHim,bool? isGroupMessage) 
        {
            try
            {
                
                // Reset parameter list
                _parameterList.Reset();

                _parameterList.Add("@Message", message);
                

                //isAdminSent 1 ise mesaj admin yollamıştır, 0 ise normal kullanıcı yollamıştır.
                if (isAdminSent == "1")
                {
                    _parameterList.Add("@isAdminMessage", true);

                    if (isGroupMessage == true)
                    {
                        _parameterList.Add("@Groupname", adminSentToHim);
                        _parameterList.Add("@Username", username);
                    }
                    else 
                    {
                        _parameterList.Add("@Username", adminSentToHim);
                    }

                    
                    // _parameterList.Add("@AdminSentToHim", adminSentToHim);
                }
                else { 
                    _parameterList.Add("@isAdminMessage", false);

                    if (isGroupMessage == true)
                    {
                        _parameterList.Add("@Groupname", adminSentToHim);
                        _parameterList.Add("@Username", username);
                    }
                    else
                    {
                        _parameterList.Add("@Username", username);
                    }
                }

                _databaseExecutions.ExecuteQuery("Sp_MessageCreate", _parameterList);

            }
            catch (Exception ex)
            {
                
            }
        }
       
    }
}
