using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMHCWebsite.Library.MeetupAPI.Entities
{
    [Serializable]
    public class MeetupEventEntity
    {
        public Int64 Created { get; set; }
        public Int64 Duration { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public Int64 Time { get; set; }
        [JsonProperty("local_date")]
        public string LocalDateStr { get; set; }
        [JsonProperty("local_time")]
        public string LocalTimeStr { get; set; }
        public Int64 Updated { get; set; }
        [JsonProperty("utc_offset")]
        public Int64 UTCOffset { get; set; }
        [JsonProperty("waitlist_count")]
        public Int64 WaitlistCount { get; set; }
        [JsonProperty("yes_rsvp_count")]
        public Int64 YesRSVPCount { get; set; }
        public MeetupVenueEntity Venue { get; set; }
        public MeetupGroupEntity Group { get; set; }
        [JsonProperty("link")]
        public string Url { get; set; }
        public string Description { get; set; }
        public string Visibility { get; set; }

        public DateTime EventStart
        {
          get { return LocalDateTime(); }
        }

        private DateTime LocalDateTime()
        {
            DateTime eventStart = new DateTime();

            if (LocalDateStr != null && LocalDateStr != string.Empty && LocalTimeStr != null && LocalTimeStr != string.Empty)
            {
                eventStart = DateTime.Parse(LocalDateStr + " " + LocalTimeStr);
            }

            return eventStart;
        }
    }
}
