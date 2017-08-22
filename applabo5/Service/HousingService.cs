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
    public class HousingService
    {
        private HttpClient httpClient;
        private string token;

        public HousingService()
        {
            httpClient = new HttpClient();
            token = LoginService.GetUniqueToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<IEnumerable<Housing>> GetAllHousings()
        {
            IEnumerable<Housing> HousingsList = HousingConverter(JArray.Parse(await httpClient.GetStringAsync(new Uri("https://smartcity-webapp.azurewebsites.net/api/housings"))));
            HousingsList = HousingsList.OrderBy(h => h.AddDate).ToList();
            HousingsList.Reverse();

            return HousingsList;
        }

        public async Task<IEnumerable<Housing>> GetAllUserHousings(String userID)
        {
            IEnumerable<Housing> HousingsList = HousingConverter(JArray.Parse(await httpClient.GetStringAsync(new Uri("https://smartcity-webapp.azurewebsites.net/api/housings/user/" + userID + "/"))));
            HousingsList = HousingsList.OrderBy(h => h.AddDate).ToList();
            HousingsList.Reverse();

            return HousingsList;
        }

        public async Task<Housing> GetOneHousing(int HousingID)
        {
            IEnumerable<Housing> liste = HousingConverter(JArray.Parse("[" + await httpClient.GetStringAsync(new Uri("https://smartcity-webapp.azurewebsites.net/api/housings/" + HousingID))+ "]"));
            return liste.First();
        }

        public IEnumerable<Housing> HousingConverter (JArray source)
        {
            return source.Children().Select(d => new Housing()
            {
                ID = d["ID"].Value<int>(),
                Host = new User()
                {
                    ID = d["Host"]["ID"].Value<String>(),
                    PassWord = d["Host"]["PassWord"].Value<String>(),
                    FirstName = d["Host"]["FirstName"].Value<String>(),
                    LastName = d["Host"]["LastName"].Value<String>(),
                    Number = d["Host"]["Number"].Value<string>(),
                    PostBox = d["Host"]["PostBox"].Value<int>(),
                    Street = d["Host"]["Street"].Value<string>(),
                    ZipCode = d["Host"]["ZipCode"].Value<int>(),
                    City = d["Host"]["City"].Value<string>(),
                    Country = d["Host"]["Country"].Value<string>(),
                    PhoneNumber = d["Host"]["PhoneNumber"].Value<String>(),
                    BirthDate = d["Host"]["BirthDate"].Value<DateTime>(),
                    EmailAddress = d["Host"]["EmailAddress"].Value<String>(),
                    Picture = d["Host"]["Picture"].Value<String>(),
                    RegistrationDate = d["Host"]["RegistrationDate"].Value<DateTime>(),
                    LastSignInDate = d["Host"]["LastSignInDate"].Value<DateTime>(),
                    Role = new Role()
                    {
                        ID = d["Host"]["Role"]["ID"].Value<int>(),
                        Title = d["Host"]["Role"]["Title"].Value<String>(),
                        UserRight = d["Host"]["Role"]["UserRight"].Value<Boolean>(),
                        HousingRight = d["Host"]["Role"]["HousingRight"].Value<Boolean>()
                    }
                },
                StartDate = d["StartDate"].Value<DateTime>(),
                EndDate = d["EndDate"].Value<DateTime>(),
                AddDate = d["AddDate"].Value<DateTime>(),
                EditDate = d["EditDate"].Value<DateTime>(),
                SpaceLocalization = d["SpaceLocalization"].Value<string>(),
                Description = d["Description"].Value<string>(),
                Pictures = d["Pictures"].Value<string>(),
                BedType = d["BedType"].Value<int>(),
                Number = d["Number"].Value<string>(),
                PostBox = d["PostBox"].Value<int>(),
                Street = d["Street"].Value<string>(),
                ZipCode = d["ZipCode"].Value<int>(),
                City = d["City"].Value<string>(),
                Wifi = d["Wifi"].Value<Boolean>(),
                Kitchen = d["Kitchen"].Value<Boolean>(),
                Office = d["Office"].Value<Boolean>(),
                Toilet = d["Toilet"].Value<Boolean>(),
                Shower = d["Shower"].Value<Boolean>()
            });
        }

        public async Task<IEnumerable<Housing>> GetSearchResults(SearchInfo informations)
        {
            List<Housing> HousingsList = new List<Housing>();
            foreach (var house in await GetAllHousings())
            {
                if (house.StartDate <= informations.StartDate && house.EndDate >= informations.EndDate)
                {
                    if (house.Wifi == informations.Wifi         &&
                        house.Kitchen == informations.Kitchen   &&
                        house.Office == informations.Office     &&
                        house.Shower == informations.Shower     &&
                        house.Toilet == informations.Toilet     &&
                        house.Wifi == informations.Wifi         &&
                        house.BedType == informations.BedType)
                    {
                        HousingsList.Add(house);
                    }
                }
            }

            return HousingsList;
        }

        public async Task AddOneHousing(HousingPost Housing)
        {
            var jsonRequest = JsonConvert.SerializeObject(Housing);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

            try
            {
                var response = await httpClient.PostAsync("https://smartcity-webapp.azurewebsites.net/api/housings/", content);
                await new MessageDialog("Annonce crée avec succès", "Création réussie").ShowAsync();
            }
            catch (HttpRequestException)
            {
                await new MessageDialog("Création de l'annonce impossible", "Erreur").ShowAsync();
            }
        }

        public async Task EditHousing(int HousingID, HousingPost HousingPost)
        {
            var jsonRequest = JsonConvert.SerializeObject(HousingPost);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

            try
            {
                var response = await httpClient.PutAsync("https://smartcity-webapp.azurewebsites.net/api/housings/"+ HousingID, content);
                await new MessageDialog("Annonce modifiée avec succès", "Modification réussie").ShowAsync();
            }
            catch (HttpRequestException)
            {
                await new MessageDialog("Modification de l'annonce impossible", "Erreur").ShowAsync();
            }
        }

        public async Task DeleteOneHousing(int HousingID)
        {
            try
            {
                var response = await httpClient.DeleteAsync("https://smartcity-webapp.azurewebsites.net/api/housings/" + HousingID);
                await new MessageDialog("Annonce suppriméé avec succès", "Suppression réussie").ShowAsync();
            }
            catch (HttpRequestException)
            {
                await new MessageDialog("Suppression de l'annonce impossible", "Erreur").ShowAsync();
            }
        }
    }
}
