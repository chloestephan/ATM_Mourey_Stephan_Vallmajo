using System;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Api
{
    public static class ApiConnection
    {
        private const string URL = "https://api.currencyfreaks.com/latest?apikey=864b20f3818a45c19d23a98bd1a3f1ac";
        public static JObject ApiFetch()
        {

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(URL).Result;
            if (response.IsSuccessStatusCode)
            {
                var dataObjects = response.Content.ReadAsStringAsync().Result;

                JObject data = (JObject)JsonConvert.DeserializeObject(dataObjects);
                return data;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }
        }

    }
}
