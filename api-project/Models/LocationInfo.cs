using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_project.Models
{
    public class LocationInfo
    {
        public string Region { get; set; }
        public bool Valid { get; set; }
        public string HostName { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }
    }
}