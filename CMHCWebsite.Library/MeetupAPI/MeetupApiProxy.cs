using CMHCWebsite.Library.MeetupAPI.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace CMHCWebsite.Library.MeetupAPI
{
    public class MeetupApiProxy
    {
        private string BaseURI { get { return @"https://api.meetup.com/"; } }
        private string GroupId { get { return "107160"; } }
        private string GroupUrl { get { return "Columbus-Mental-Health-Support-and-Social-Meetup"; } }
        private string ApiKey { get { return "642a42d14632e292461297723751349"; } }

        public MeetupApiProxy() { }

        public List<MeetupEventEntity> GetUpcomingEvents()
        {
            List<MeetupEventEntity> events = new List<MeetupEventEntity>();

            try
            {
                // Build URL
                string url = BaseURI + GroupUrl + "/events?sign=true&photo-host=public&page=20&sign=true&key=" + ApiKey;

                // Start Request
                var request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";
                request.Headers.Clear();

                string content = string.Empty;

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();
                        }
                    }
                }

                events = JsonConvert.DeserializeObject<List<MeetupEventEntity>>(content);
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
