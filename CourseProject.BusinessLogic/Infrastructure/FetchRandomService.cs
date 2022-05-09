using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;

namespace CourseProject.BusinessLogic.Infrastructure
{
    public class FetchRandomService : IRandomService
    {
        private readonly WebClient webClient;
        public FetchRandomService()
        {
            webClient = new WebClient();
        }
        public int Next(int maxValue)
        {
            string data = webClient.DownloadString(@"https://random-data-api.com/api/number/random_number");
            var json = JsonSerializer.Deserialize<JsonElement>(data);

            var value = json.GetProperty("number").GetInt64();

            return (int)(value % maxValue);
        }
    }
}
