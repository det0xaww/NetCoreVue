using NetCoreVue.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreVue.ViewModels
{
    public class TeamViewModel
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

        public static implicit operator TeamViewModel(Team model)
        {
            return new TeamViewModel {
                abbreviation = model.abbreviation,
                city = model.city,
                conference = model.conference,
                division = model.division,
                full_name = model.full_name,
                id = model.id,
                name = model.name 
            };
        }

        public static implicit operator Team(TeamViewModel model)
        {
            return new Team
            {
                abbreviation = model.abbreviation,
                city = model.city,
                conference = model.conference,
                division = model.division,
                full_name = model.full_name,
                id = model.id,
                name = model.name
            };
        }
    }
}
