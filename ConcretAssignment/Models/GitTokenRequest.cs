using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConcretAssignment.Models
{
    public class GitTokenRequest
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }
    }
}