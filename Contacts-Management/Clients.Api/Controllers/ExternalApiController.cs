using Clients.Api.ExternalServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Clients.Api.Controllers
{
    // This class just demonstrates how to call external API endpoints

    [ApiController]
    [Route("/api/predictAge")]
    public class ExternalApiController : ControllerBase
    {
        private readonly AgeApi _ageApi;

        public ExternalApiController(AgeApi ageApi)
        {
            _ageApi = ageApi;
        }

        [HttpGet]
        public async Task<ActionResult<string>> PredictAgeFromName([FromQuery] string name)
        {
            var age = await _ageApi.GetAge(name);

            return Ok($"Predicted age for {name} = {age}");
        }
    }
}
