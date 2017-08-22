using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using HomeSnailHome.Model;
using Newtonsoft.Json;
using System.Text;
using Windows.UI.Popups;
using System.Net.Http.Headers;

namespace HomeSnailHome.Service
{
    public class MessageService
    {
        private HttpClient httpClient;
        private string token;

        public MessageService()
        {
            httpClient = new HttpClient();
            token = LoginService.GetUniqueToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public IEnumerable<Message> MessageConverter(JArray source)
        {
             return source.Children().Select(d => new Message()
            {
                ID = d["ID"].Value<int>(),
                Sender = new User()
                {
                    ID = d["Sender"]["ID"].Value<String>(),
                    FirstName = d["Sender"]["FirstName"].Value<String>(),
                    LastName = d["Sender"]["LastName"].Value<String>(),
                    EmailAddress = d["Sender"]["EmailAddress"].Value<String>(),
                    Picture = d["Sender"]["Picture"].Value<String>()
                },
                Reciever = new User()
                {
                    ID = d["Reciever"]["ID"].Value<String>(),
                    FirstName = d["Reciever"]["FirstName"].Value<String>(),
                    LastName = d["Reciever"]["LastName"].Value<String>(),
                    EmailAddress = d["Reciever"]["EmailAddress"].Value<String>(),
                    Picture = d["Reciever"]["Picture"].Value<String>()
                },
                SendDate = d["SendDate"].Value<DateTime>(),
                Housing = HousingValue(d["Housing"]),
                Content = d["Content"].Value<string>()
            });
        }

        private Housing HousingValue(JToken jToken)
        {
            if (jToken is JValue) return new Housing() { ID = 0 };
            else return new Housing() { ID = jToken["ID"].Value<int>() };
        }

        public async Task<IEnumerable<Message>> GetOneConversationMessages(String correspondent, int subject)
        {
            IEnumerable<Message> MessageList = MessageConverter(JArray.Parse(await httpClient.GetStringAsync(new Uri("https://smartcity-webapp.azurewebsites.net/api/messages/Conversation/" + correspondent + "/" + subject))));
            MessageList = MessageList.OrderBy(m => m.SendDate);
            return MessageList.Reverse();
        }

        public async Task<IEnumerable<Conversation>> GetAllConversationsUser(String userID)
        {
            List<Message> MessagesList = new List<Message>();
            IEnumerable<Message> messages = MessageConverter(JArray.Parse(await httpClient.GetStringAsync(new Uri("https://smartcity-webapp.azurewebsites.net/api/messages/user/" + userID + "/"))));

            foreach (var mess in messages)
            {
                if (mess.Housing == null)
                {
                    mess.Housing = new Housing() { ID = 0 };
                }
                MessagesList.Add(mess);
            }

            MessagesList = MessagesList.OrderBy(m => m.SendDate).ToList();
            MessagesList.Reverse();
            MessagesList = MessagesList.OrderBy(m => m.Housing.ID).ToList();
            
            List<Conversation> ConversationList = new List<Conversation>();

            if (MessagesList.Count > 0)
            {
                Conversation ConversationAct = new Conversation() { Messages = new List<Message>() };

                if (MessagesList[0].Reciever.ID == userID) ConversationAct.Correspondant = MessagesList[0].Sender;
                else ConversationAct.Correspondant = MessagesList[0].Reciever;

                ConversationAct.Subject = MessagesList[0].Housing;
                User Correspondant;

                foreach (var mess in MessagesList)
                {
                    if (mess.Reciever.ID == userID) Correspondant = mess.Sender;
                    else Correspondant = mess.Reciever;

                    if (ConversationAct.Correspondant.ID != Correspondant.ID || mess.Housing.ID != ConversationAct.Subject.ID)
                    {
                        ConversationList.Add(ConversationAct);
                        ConversationAct = new Conversation()
                        {
                            Messages = new List<Message>(),
                            Correspondant = Correspondant,
                            Subject = mess.Housing
                        };
                    }
                    ConversationAct.Messages.Add(mess);
                }

                ConversationList.Add(ConversationAct);

                foreach (var conv in ConversationList)
                {
                    conv.Messages = conv.Messages.OrderBy(m => m.SendDate).ToList();
                    conv.Messages.Reverse();
                }

                ConversationList = ConversationList.OrderBy(con => con.Messages[0].SendDate).ToList();
                ConversationList.Reverse();
            }

            return ConversationList;
        }
        
        public async Task SendMessage(String Sender, String Reciever, int Subject, String Text)
        {
            MessagePost message = new MessagePost()
            {
                SenderID = Sender,
                RecieverID = Reciever,
                Content = Text,
                HousingID = Subject
            };

            var jsonRequest = JsonConvert.SerializeObject(message);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

            try
            {
                var response = await httpClient.PostAsync("https://smartcity-webapp.azurewebsites.net/api/messages/", content);
            }
            catch (HttpRequestException)
            {
                await new MessageDialog("Envoi impossible", "Erreur").ShowAsync();
            }
        }
    }
}