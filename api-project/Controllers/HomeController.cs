using api_project.Models;
using api_project.Services;
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
        private string _defaultIp = "130.215.6.1";

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
            var userIp = Request.IsLocal ? _defaultIp : Request.UserHostAddress;
            var geoIpEndpoint = WebConfigurationManager.AppSettings["GeoIpEndpoint"];

            var locationService = new LocationService(geoIpEndpoint);

            var locationInfoRaw = await locationService.GetLocationInfoRaw(userIp);

            return Content(locationInfoRaw, "application/json");
        }

        public async Task<ActionResult> GetPicturesNearMe(decimal latitude, decimal longitude)
        {
            var flickrApiKey = WebConfigurationManager.AppSettings["Flickr.ApiKey"];
            var flickrEndpoint = WebConfigurationManager.AppSettings["Flickr.Endpoint"];

            var picturesService = new PhotoService(flickrApiKey, flickrEndpoint);

            var pictureListRaw = await picturesService.GetPictureListRaw(latitude, longitude);

            return Content(pictureListRaw, "application/javascript");
        }
    }
}