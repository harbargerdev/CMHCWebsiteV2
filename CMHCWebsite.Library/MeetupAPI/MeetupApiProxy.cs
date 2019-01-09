using CMHCWebsite.Library.MeetupAPI.Entities;
using System;
using System.Collections.Generic;

namespace CMHCWebsite.Library.MeetupAPI
{
    public class MeetupApiProxy
    {
        private string BaseURI { get { return @"https://api.meetup.com"; } }
        private string GroupId { get { return "107160"; } }
        private string GroupUrl { get { return "Columbus-Mental-Health-Support-and-Social-Meetup"; } }

        public MeetupApiProxy() { }

        public List<MeetupEventEntity> GetUpcomingEvents()
        {
            List<MeetupEventEntity> events = new List<MeetupEventEntity>();

            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return events;
        }

        private MeetupEventEntity RawObjectToMeetupEventEntity(object rawObj)
        {
            MeetupEventEntity eventEntity = null;

            return eventEntity;
        }
    }
}
