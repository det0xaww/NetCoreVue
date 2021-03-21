using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreVue.Models
{
    public class Team
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [Required, JsonProperty("abbreviation")]
        public string abbreviation { get; set; }
        [Required, JsonProperty("city")]
        public string city { get; set; }
        [Required, JsonProperty("conference")]
        public string conference { get; set; }
        [Required, JsonProperty("division")]
        public string division { get; set; }
        [Required, JsonProperty("full_name")]
        public string full_name { get; set; }
        [Required, JsonProperty("name")]
        public string name { get; set; }
    }
}
