using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMHCWebsite.Library.MeetupAPI.Entities
{
    public class MeetupVenueEntity
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public decimal Lat { get; set; }
        public decimal Lon { get; set; }
        public bool Repinned { get; set; }
        [JsonProperty("address_1")]
        public string Address1 { get; set; }
        [JsonProperty("address_2")]
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        [JsonProperty("localized_country_name")]
        public string LocalizedCountryName { get; set; }
    }
}
