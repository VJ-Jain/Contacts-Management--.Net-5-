using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Clients.Api.ExternalServices
{
    public class AgeApi
    {
        private readonly string _ageApiBaseUrl;

        public AgeApi(IOptions<AgeApiConfiguration> options)
        {
            _ageApiBaseUrl = options.Value.BaseUrl;
        }

        public virtual async Task<int> GetAge(string name)
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync($"{_ageApiBaseUrl}/?name={name}");
            var responseBody = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<AgeApiUserDto>(responseBody);

            return user.Age;
        }
    }
}
