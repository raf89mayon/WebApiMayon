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

namespace HomeSnailHome.Service
{
    public class LocalService
    {
        private HttpClient httpClient;
        private string token;

        public LocalService()
        {
            httpClient = new HttpClient();
            token = LoginService.GetUniqueToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<IEnumerable<InfoLocal>> GetEditedLocalInfos(Housing housing)
        {
            try
            {
                return InfoLocalConverter(JArray.Parse("[" + await httpClient.GetStringAsync(new Uri("https://maps.googleapis.com/maps/api/geocode/json?address=" + housing.Number + "+" + housing.Street + "+" + housing.ZipCode + "+" + housing.City + "&key=AIzaSyCT-lt1TsM7W-Ri4xxpTfTgGvrNHhdCX-k")) + "]"));
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<InfoLocal>> GetLocalization(HousingPost housing)
        {
            try
            {
                return InfoLocalConverter(JArray.Parse("[" + await httpClient.GetStringAsync(new Uri("https://maps.googleapis.com/maps/api/geocode/json?address=" + housing.Number + "+" + housing.Street + "+" + housing.ZipCode + "+" + housing.City + "&key=AIzaSyCT-lt1TsM7W-Ri4xxpTfTgGvrNHhdCX-k")) + "]"));
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public IEnumerable<InfoLocal> InfoLocalConverter(JArray obj)
        {
            IEnumerable<InfoLocal> infos = obj.Children().Select(d => new InfoLocal()
            {
                Status = d["status"].Value<String>()
            });

            if (infos.First().Status.Equals("OK"))
            {
                infos = obj.Children().Select(d => new InfoLocal()
                {
                    Status = d["status"].Value<String>(),
                    location_lat = d["results"][0]["geometry"]["location"]["lat"].Value<double>(),
                    location_lon = d["results"][0]["geometry"]["location"]["lng"].Value<double>(),
                    northeast_lat = d["results"][0]["geometry"]["viewport"]["northeast"]["lat"].Value<double>(),
                    northeast_lon = d["results"][0]["geometry"]["viewport"]["northeast"]["lng"].Value<double>(),
                    southwest_lat = d["results"][0]["geometry"]["viewport"]["southwest"]["lat"].Value<double>(),
                    southwest_lon = d["results"][0]["geometry"]["viewport"]["southwest"]["lng"].Value<double>()
                });
                return infos;
            }
            else
                return null;
        }
    }
}