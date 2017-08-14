using Locs4Youth.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Locs4Youth.Models
{
    public class SharedLocation
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
    }

    /// <summary>
    /// Fetches items from partners
    /// </summary>
    public class Partners
    {
        private const string 
            API_MATTHIAS = "http://matthiashoebeke.ikdoeict.net/api/SharedLocations",
            API_LAURENS = "http://jlb.decocklaurens.ikdoeict.net/api/SharedLocations";


        /// <summary>
        /// Partner 1
        /// </summary>
        /// <param name="model">Filter Model</param>
        /// <returns>List of shared locations</returns>
        public static List<SharedLocation> Matthias(FilterModel model)
        {
            string url = $"{API_MATTHIAS}?location={model.Title}&address={(model.Region.HasValue ? model.Region.GetEnumDisplay() : null)}&start_date={model.From}&end_date={model.To}&guests={model.Capacity}";
            var results = (List<SharedLocation>)JsonConvert.DeserializeObject(GetJson(url), typeof(List<SharedLocation>));
            return results;
        }

        /// <summary>
        /// Partner 2
        /// </summary>
        /// <param name="model">Filter Model</param>
        /// <returns>List of shared locations</returns>
        public static List<SharedLocation> Laurens(FilterModel model)
        {
            string url = $"{API_LAURENS}?location={model.Title}&address={(model.Region.HasValue ? model.Region.GetEnumDisplay() : null)}&start_date={model.From}&end_date={model.To}&guests={model.Capacity}";
            var results = (List<SharedLocation>)JsonConvert.DeserializeObject(GetJson(url), typeof(List<SharedLocation>));
            return results;
        }

        /// <summary>
        /// Retrieves JSON and parses
        /// </summary>
        /// <param name="url">URL to read</param>
        /// <returns>JSON (as string)</returns>
        private static string GetJson(string url)
        {
            var webClient = new WebClient();
            string json;
            try
            {
                json = webClient.DownloadString(url);
            }
            catch (Exception)
            {
                json = "[]";
            }
            return json;
        }
    }
}