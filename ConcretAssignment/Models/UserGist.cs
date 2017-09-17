using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConcretAssignment.Models
{
    public class UserGist
    {
        [JsonProperty("url")]
        public string URL { get; set; }
        [JsonProperty("forks_url")]
        public string ForksUrl { get; set; }
        [JsonProperty("commits_url")]
        public string commits_url { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
        [JsonProperty("created_at")]
        public DateTime Created { get; set; }
        [JsonProperty("updated_at")]
        public DateTime Updated { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}