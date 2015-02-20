using api_project.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace api_project.Controllers
{
    public class HomeController : Controller
    {
        // WPI
        public string _defaultIp = "130.215.6.1";

        public string ApiKey { get; set; }

        public HomeController()
        {
            ApiKey = WebConfigurationManager.AppSettings["Mashape.ApiKey"];
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async Task<ActionResult> GetPicturesNearMe()
        {
            var locationInfo = await GetLocationInfo();
            var pictures = await GetPictureInfo(locationInfo.Latitude, locationInfo.Longitude);

            return Content(pictures, "application/json");
        }

        private async Task<LocationInfo> GetLocationInfo()
        {
            var ipEndpoint = WebConfigurationManager.AppSettings["Mashape.IpEndpoint"];
            var userIp = Request.IsLocal ? _defaultIp : Request.UserHostAddress;

            var request = new RestRequest(Method.POST);

            request.AddHeader("X-Mashape-Key", ApiKey);
            request.AddParameter("ip", userIp);

            var restClient = new RestClient(ipEndpoint);

            // Sample Repsonse:
            //{
            //  "region": "Virginia",
            //  "valid": true,
            //  "hostname": "",
            //  "longitude": -77.48748779296875,
            //  "latitude": 39.04372024536133,
            //  "country-code": "US",
            //  "city": "Ashburn",
            //  "country": "United States"
            //}

            var response = await restClient.ExecuteTaskAsync<dynamic>(request);

            // Not perfect, but good enough
            return JsonConvert.DeserializeObject<LocationInfo>(response.Content);
        }

        private async Task<string> GetPictureInfo(decimal latitude, decimal longitude)
        {
            var ipEndpoint = WebConfigurationManager.AppSettings["Mashape.ImageEndpoint"];
            var userIp = Request.IsLocal ? "134.170.188.221" : Request.UserHostAddress;

            var request = new RestRequest(Method.POST);

            request.AddHeader("X-Mashape-Key", ApiKey);

            request.AddQueryParameter("lang", "en");

            // North, South, East, West for a bounding box of locations.
            request.AddQueryParameter("n", (latitude + .5m).ToString());
            request.AddQueryParameter("s", (latitude - .5m).ToString());
            request.AddQueryParameter("e", (longitude + .5m).ToString());
            request.AddQueryParameter("w", (longitude - .5m).ToString());

            var restClient = new RestClient(ipEndpoint);

            var response = await restClient.ExecuteTaskAsync<dynamic>(request);

            return response.Content;
        }
    }
}