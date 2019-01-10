using System;
using System.Collections.Generic;
using System.Text;

namespace CMHCWebsite.Library.MeetupAPI.Entities
{
    public class MeetupGroupEntity
    {
        public Int64 Created { get; set; }
        public string Name { get; set; }
        public Int64 Id { get; set; }
        public string JoinMode { get; set; }
        public long Lat { get; set; }
        public long Lon { get; set; }
        public string UrlName { get; set; }
        public string Who { get; set; }
        public string LocalizedLocation { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string TimeZone { get; set; }
    }
}
