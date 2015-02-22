using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace api_project.Services
{
    public class LocationService
    {
        private string _geoIpEndpoint;

        public LocationService(string geoIpEndpoint)
        {
            _geoIpEndpoint = geoIpEndpoint;
        }

        public async Task<string> GetLocationInfoRaw(string ipAddress)
        {
            var request = new RestRequest(Method.GET);

            var restClient = new RestClient(_geoIpEndpoint + ipAddress + "/json");

            // Sample response:
            //{
            //    "ip": "71.248.188.119",
            //    "hostname": "static-71-248-188-119.bstnma.fios.verizon.net",
            //    "city": "Westborough",
            //    "region": "Massachusetts",
            //    "country": "US",
            //    "loc": "42.2658,-71.6105",
            //    "org": "AS701 MCI Communications Services, Inc. d/b/a Verizon Business",
            //    "postal": "01581"
            //}

            var response = await restClient.ExecuteTaskAsync(request);

            return response.Content;
        }
    }
}