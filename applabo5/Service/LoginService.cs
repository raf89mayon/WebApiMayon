using HomeSnailHome.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeSnailHome.Service
{
    public class LoginService
    {
        private static string token;

        public async Task<String> GetToken(String enteredEmail, String enteredPassword)
        {
            try
            {
                var client = new HttpClient();
                var requestBody = new List<KeyValuePair<string, string>>();
                requestBody.Add(new KeyValuePair<string, string>("Username", enteredEmail));
                requestBody.Add(new KeyValuePair<string, string>("Password", enteredPassword));
                requestBody.Add(new KeyValuePair<string, string>("grant_type", "password"));

                var content = new FormUrlEncodedContent(requestBody);

                var response = await client.PostAsync(new Uri("https://smartcity-webapp.azurewebsites.net/token"), content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic data = JObject.Parse(responseBody);
                    string tokenReceived = data.access_token;
                    token = tokenReceived;
                    return tokenReceived;
                }
                else
                {
                    throw new ApiRequestFailedException();
                }
            }
            catch (ApiRequestFailedException)
            {
                throw;
            }
        }

        public static string GetUniqueToken()
        {
            return token;
        }
    }
}
