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
    public class NotationService
    {
        private HttpClient httpClient;
        private string token;

        public NotationService()
        {
            httpClient = new HttpClient();
            token = LoginService.GetUniqueToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public IEnumerable<Notation> NotationConverter(JArray source)
        {
            return source.Children().Select(d => new Notation()
            {
                ID = d["ID"].Value<int>(),
                Origin = new User()
                {
                    ID = d["Origin"]["ID"].Value<String>(),
                    FirstName = d["Origin"]["FirstName"].Value<String>(),
                    LastName = d["Origin"]["LastName"].Value<String>(),
                    Picture = d["Origin"]["Picture"].Value<String>()
                },
                Housing = new Housing()
                {
                    ID = d["Housing"]["ID"].Value<int>(),
                    Pictures = d["Housing"]["Pictures"].Value<string>(),
                    Number = d["Housing"]["Number"].Value<string>(),
                    PostBox = d["Housing"]["PostBox"].Value<int>(),
                    Street = d["Housing"]["Street"].Value<string>(),
                    ZipCode = d["Housing"]["ZipCode"].Value<int>(),
                    City = d["Housing"]["City"].Value<string>()
                },
                DateNotation = d["DateNotation"].Value<DateTime>(),
                Comment = d["Comment"].Value<string>(),
                Quotation = d["Quotation"].Value<float>()
            });
        }

        public async Task<IEnumerable<Notation>> GetAllNotationsOneHousing(int HousingID)
        {
            IEnumerable<Notation> NotationList = NotationConverter(JArray.Parse(await httpClient.GetStringAsync(new Uri("https://smartcity-webapp.azurewebsites.net/api/notations/Housing/" + HousingID))));
            return NotationList = NotationList.OrderBy(n => n.DateNotation).ToList();
        }

        public async Task SendNotation(NotationPost notationPost)
        {
            var jsonRequest = JsonConvert.SerializeObject(notationPost);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

            try
            {
                var response = await httpClient.PostAsync("https://smartcity-webapp.azurewebsites.net/api/notations/", content);
                await new MessageDialog("Note créée avec succès", "Création réussie").ShowAsync();
            }
            catch (HttpRequestException)
            {
                await new MessageDialog("Envoi impossible de la note", "Erreur").ShowAsync();
            }
        }
    }
}
