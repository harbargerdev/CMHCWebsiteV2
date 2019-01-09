using System;
using System.Collections.Generic;
using System.Text;

namespace CMHCWebsite.Library.MeetupAPI.Entities
{
    [Serializable]
    public class MeetupEventEntity
    {
        public int Created { get; set; }
        public int Duration { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int Time { get; set; }
        public string LocalDate { get; set; }
        public string LocalTime { get; set; }
        public int Updated { get; set; }
        public int UTCOffset { get; set; }
        public int WaitlistCount { get; set; }
        public int YesRSVPCount { get; set; }
        public MeetupVenueEntity Venue { get; set; }
        public MeetupGroupEntity Group { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Visibility { get; set; }
    }
}
