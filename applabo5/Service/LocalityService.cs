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
    public class LocalityService
    {
        private HttpClient httpClient;
        private string token;

        public LocalityService()
        {
            httpClient = new HttpClient();
            token = LoginService.GetUniqueToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<IEnumerable<Locality>> GetAllLocalities()
        {
            IEnumerable<Locality> list = LocalityConverter(JArray.Parse(await httpClient.GetStringAsync(new Uri("https://smartcity-webapp.azurewebsites.net/api/localities"))));
            return list.OrderBy(loc => loc.Zip).ToList();
        }

        public IEnumerable<Locality> LocalityConverter(JArray source)
        {
            return source.Children().Select(d => new Locality()
            {
                Zip = d["Zip"].Value<int>(),
                Name = d["Name"].Value<string>()
            });
        }
    }
}
