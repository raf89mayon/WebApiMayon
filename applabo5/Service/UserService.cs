using HomeSnailHome.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace HomeSnailHome.Service
{
    public class UserService
    {
        private HttpClient httpClient;
        private string token;

        public UserService()
        {
            httpClient = new HttpClient();
            token = LoginService.GetUniqueToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return UserConverter(JArray.Parse(await httpClient.GetStringAsync(new Uri("https://smartcity-webapp.azurewebsites.net/api/users/"))));
        }
        
        public IEnumerable<User> UserConverter(JArray source)
        {
            return source.Children().Select(d => new User()
            {
                ID = d["ID"].Value<String>(),
                PassWord = d["PassWord"].Value<String>(),
                FirstName = d["FirstName"].Value<String>(),
                LastName = d["LastName"].Value<String>(),
                Number = d["Number"].Value<string>(),
                PostBox = d["PostBox"].Value<int>(),
                Street = d["Street"].Value<string>(),
                ZipCode = d["ZipCode"].Value<int>(),
                City = d["City"].Value<string>(),
                Country = d["Country"].Value<string>(),
                PhoneNumber = d["PhoneNumber"].Value<String>(),
                BirthDate = d["BirthDate"].Value<DateTime>(),
                EmailAddress = d["EmailAddress"].Value<String>(),
                Picture = d["Picture"].Value<String>(),
                RegistrationDate = d["RegistrationDate"].Value<DateTime>(),
                LastSignInDate = d["LastSignInDate"].Value<DateTime>(),
                Role = new Role()
                {
                    ID = d["Role"]["ID"].Value<int>(),
                    Title = d["Role"]["Title"].Value<String>(),
                    UserRight = d["Role"]["UserRight"].Value<Boolean>(),
                    HousingRight = d["Role"]["HousingRight"].Value<Boolean>()
                }
            });
        }

        public async void DeleteUser(int id)
        {
            await httpClient.DeleteAsync(new Uri("https://smartcity-webapp.azurewebsites.net/api/users/" + id));
        }

        public async Task AddOneUser(UserPost userToAdd, String PassWord)
        {
            UserAccount AccountToAdd = new UserAccount()
            {
                EMail = userToAdd.ID,
                Password = PassWord,
                ConfirmPassword = PassWord
            };

            var jsonRequestAccount = JsonConvert.SerializeObject(AccountToAdd);
            var contentAccount = new StringContent(jsonRequestAccount, Encoding.UTF8, "text/json");

            try
            {
                var response = await httpClient.PostAsync("https://smartcity-webapp.azurewebsites.net/api/Account/Register/", contentAccount);

                var jsonRequest = JsonConvert.SerializeObject(userToAdd);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

                try
                {
                    response = await httpClient.PostAsync("https://smartcity-webapp.azurewebsites.net/api/users/", content);
                    await new MessageDialog("Compte utilisateur crée avec succès", "Création réussie").ShowAsync();
                }
                catch (HttpRequestException)
                {
                    await new MessageDialog("Création du compte utilisateur impossible", "Erreur").ShowAsync();
                }
            }
            catch (HttpRequestException)
            {
                await new MessageDialog("Création du compte utilisateur impossible", "Erreur").ShowAsync();
            }
        }
        
        public async Task<User> GetOneUser(String userID)
        {
            try
            {
                return JsonConvert.DeserializeObject<User>(await httpClient.GetStringAsync(new Uri("https://smartcity-webapp.azurewebsites.net/api/users/" + userID + "/")));
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}