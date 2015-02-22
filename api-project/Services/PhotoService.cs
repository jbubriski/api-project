﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace api_project.Services
{
    public class PhotoService
    {
        private string _flickrApiKey;
        private string _flickrEndpoint;

        public PhotoService(string flickrApiKey, string flickrEndpoint)
        {
            _flickrApiKey = flickrApiKey;
            _flickrEndpoint = flickrEndpoint;
        }

        public async Task<string> GetPictureListRaw(decimal latitude, decimal longitude)
        {
            var request = new RestRequest(Method.POST);

            request.AddQueryParameter("api_key", _flickrApiKey);

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

            var restClient = new RestClient(_flickrEndpoint);

            var response = await restClient.ExecuteTaskAsync(request);

            return response.Content;
        }
    }
}