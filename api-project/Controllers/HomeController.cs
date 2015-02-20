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

        public string GeoIpEndpoint { get; set; }
        public string FlickrApiKey { get; set; }
        public string FlickrEndpoint { get; set; }

        public HomeController()
        {
            GeoIpEndpoint = WebConfigurationManager.AppSettings["GeoIpEndpoint"];
            FlickrApiKey = WebConfigurationManager.AppSettings["Flickr.ApiKey"];
            FlickrEndpoint = WebConfigurationManager.AppSettings["Flickr.Endpoint"];
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public async Task<ActionResult> GetLocationInfo()
        {
            var locationInfoRaw = await GetLocationInfoRaw();

            return Content(locationInfoRaw, "application/json");
        }

        public async Task<ActionResult> GetPicturesNearMe(decimal latitude, decimal longitude)
        {
            var pictureListRaw = await GetPictureListRaw(latitude, longitude);

            return Content(pictureListRaw, "application/javascript");
        }

        private async Task<string> GetLocationInfoRaw()
        {
            var userIp = Request.IsLocal ? _defaultIp : Request.UserHostAddress;

            var request = new RestRequest(Method.GET);

            var restClient = new RestClient(GeoIpEndpoint + userIp + "/json");

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

        private async Task<string> GetPictureListRaw(decimal latitude, decimal longitude)
        {
            var request = new RestRequest(Method.POST);

            request.AddQueryParameter("api_key", FlickrApiKey);

            request.AddQueryParameter("method", "flickr.photos.search");
            request.AddQueryParameter("format", "json");

            // Safe
            request.AddQueryParameter("safe_search", "1");

            // Pictures
            request.AddQueryParameter("content_type", "1");

            // No license
            request.AddQueryParameter("license", "1");

            // North, South, East, West for a bounding box of locations.
            request.AddQueryParameter("bbox", string.Format("{0},{1},{2},{3}", (longitude - 1m), (latitude - 1m), (longitude + 1m), (latitude + 1m)));

            var restClient = new RestClient(FlickrEndpoint);

            var response = await restClient.ExecuteTaskAsync(request);

            return response.Content;
        }
    }
}