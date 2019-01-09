using System;
using System.Collections.Generic;
using System.Text;

namespace CMHCWebsite.Library.MeetupAPI.Entities
{
    public class MeetupVenueEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Lat { get; set; }
        public decimal Lon { get; set; }
        public bool Repinned { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string LocalizedCountryName { get; set; }
    }
}
