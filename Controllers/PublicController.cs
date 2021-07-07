using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TodoAppWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        [HttpGet]
        [Route("country")]
        public async Task<string> GetCountryAsync()
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://api.countrystatecity.in/v1/countries"),
                Method = HttpMethod.Get
            };
            request.Headers.Add("X-CSCAPI-KEY", "b1FrNUlxdWQ1c0dOT2JkVDhtdXFTR0xtdGdGaWx0TVhLZWZhcmtlcA==");
            HttpResponseMessage response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            
            return responseString;
        }

        [HttpGet]
        [Route("state")]
        public async Task<string> GetStateAsync(string countryName)
        {   
            string url = @"https://api.countrystatecity.in/v1/countries/"+ countryName +"/states";
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };
            request.Headers.Add("X-CSCAPI-KEY", "b1FrNUlxdWQ1c0dOT2JkVDhtdXFTR0xtdGdGaWx0TVhLZWZhcmtlcA==");
            HttpResponseMessage response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }

        [HttpGet]
        [Route("city")]
        public async Task<string> GetCityAsync(string countryName, string stateName)
        {
            string url = @"https://api.countrystatecity.in/v1/countries/" + countryName + "/states/" + stateName + "/cities";
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };
            request.Headers.Add("X-CSCAPI-KEY", "b1FrNUlxdWQ1c0dOT2JkVDhtdXFTR0xtdGdGaWx0TVhLZWZhcmtlcA==");
            HttpResponseMessage response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }
    }
}
