using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using HomeSnailHome.Model;
using System.Net.Http.Headers;

namespace HomeSnailHome.Service
{
    public class BedService
    {
        private HttpClient httpClient;
        private string token;

        public BedService()
        {
            httpClient = new HttpClient();
            token = LoginService.GetUniqueToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<IEnumerable<Bed>> GetAllBedTypes()
        {
            return BedConverter(JArray.Parse(await httpClient.GetStringAsync(new Uri("https://smartcity-webapp.azurewebsites.net/api/beds"))));
        }

        public IEnumerable<Bed> BedConverter(JArray source)
        {
            return source.Children().Select(d => new Bed()
            {
                Code = d["Code"].Value<int>(),
                Name = d["Name"].Value<string>()
            });
        }
    }
}
