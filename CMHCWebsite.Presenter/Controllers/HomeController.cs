using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CMHCWebsite.Presenter.Models;
using CMHCWebsite.Library.MeetupAPI;
using CMHCWebsite.Library.MeetupAPI.Entities;
using CMHCWebsite.Library.ContentManager;
using CMHCWebsite.Library.ContentManager.Entities;
using System.Text;

namespace CMHCWebsite.Presenter.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                ViewData["Content"] = GetContent("HomePage");
            }
            catch (Exception)
            {
            }
            return View();
        }

        public IActionResult UpcomingEvents()
        {
            MeetupApiProxy proxy = new MeetupApiProxy();
            var events = proxy.GetUpcomingEvents();

            ViewData["EventsTable"] = BuildEventsTable(events);

            return View();
        }

        public IActionResult DiscussionBoard()
        {
            ViewData["Message"] = "Discussion Board";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult History(string activeView = "Mission")
        {
            try
            {
                if (activeView.Equals("Staff"))
                    ViewData["Content"] = BuildStaffingTable(activeView);
                else
                    ViewData["Content"] = GetContent(activeView);
            }
            catch (Exception ex)
            {
            }
            return View();
        }

        public IActionResult Programs(string activeView = "Meetups")
        {
            ViewData["Content"] = GetContent(activeView);
            return View();
        }

        public IActionResult CrisisHelp()
        {
            try
            {
                ViewData["Content"] = GetContent("CrisisHelp");
            }
            catch (Exception)
            {
            }
            return View();
        }

        public IActionResult Resources(string activeView = "localResources")
        {
            ViewData["Content"] = GetContent(activeView);
            return View();
        }

        public IActionResult HelpUs(string activeView = "volunteer")
        {
            ViewData["Content"] = GetContent(activeView);
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        #region Helper Methods

        private string BuildEventsTable(List<MeetupEventEntity> events)
        {
            string html = string.Empty;

            if(events.Count > 0)
            {
                html = "<table id=\"upcomingEventsTable\" class=\"table-striped\" style=\"padding: 1px;width: 90%;\">";
                html += "<tr><th style=\"width: 15%;\">Event Start</th><th style=\"width: 30%;\">Event Title</th><th style=\"width: 5%;\">RSVPs</th><th style=\"width: 35%;\">Location Name</th><th style=\"width: 7%;\"></th></tr>";

                foreach(MeetupEventEntity mEvent in events)
                {
                    html += "<tr>";
                    // Event Date Time
                    html += "<td>" + mEvent.EventStart.ToString("MM/dd/yyyy hh:mm tt") + "</td>";
                    // Event Title
                    html += "<td>" + mEvent.Name + "</td>";
                    // RSVPs
                    html += "<td>" + mEvent.YesRSVPCount + "</td>";
                    // Location Name
                    html += "<td><a href=\"" + BuildMapsUrl(mEvent.Venue) + "\" target=\"_blank\" \">" + mEvent.Venue.Name + "</a></td>";
                    // Link
                    html += "<td><a href=\"" + mEvent.Url + "\"  target=\"_blank\" >Join Now!</a></td>";

                    html += "</tr>";
                }

                html += "</table>";
            }

            return html;
        }

        private string GetContent(string key)
        {
            ContentUtility cUtility = new ContentUtility();

            return cUtility.GetContent(ContentSource.S3, key);
        }

        private string BuildStaffingTable(string key)
        {
            StringBuilder builder = new StringBuilder();
            ContentUtility utility = new ContentUtility();

            STAFF_TYPE sType;

            switch(key)
            {
                case "Volunteer":
                    sType = STAFF_TYPE.Volunteer;
                    break;
                case "PartTime":
                    sType = STAFF_TYPE.PartTimeStaff;
                    break;
                case "Fulltime":
                    sType = STAFF_TYPE.FullTimeStaff;
                    break;
                default:
                    sType = STAFF_TYPE.All;
                    break;
            }

            var staff = utility.GetStaff(sType);

            builder.Append("<table class=\"table-striped\" id=\"staffTable\">");
            builder.Append("<tr><th style=\"width: 15%\">Staff Name</th><th style=\"width: 15%\">Type</th>" + 
                "<th style=\"width: 15%\">Role</th><th style=\"width: 25%\">Bio</th><th>Picture</th></tr>");

            foreach(StaffEntity member in staff)
            {
                string typeDesc = string.Empty;
                switch(member.Category)
                {
                    case STAFF_TYPE.FullTimeStaff:
                        typeDesc = "Full-time Staff";
                        break;
                    case STAFF_TYPE.PartTimeStaff:
                        typeDesc = "Part-time Staff";
                        break;
                    case STAFF_TYPE.Volunteer:
                        typeDesc = "Volunteer";
                        break;
                    default:
                        break;
                }


                string fullName = member.FirstName + " " + member.LastName;
                builder.Append("<tr><td>" + fullName + "</td><td>" + typeDesc + "</td><td>" + member.Role + "</td><td>" +
                    member.Bio + "</td><td><img class=\"profile\" src=\"" + member.ImgUrl + "\", alt=\"" + fullName +"\"</td>");
            }

            builder.Append("</table>");

            return builder.ToString();
        }

        private string BuildMapsUrl(MeetupVenueEntity venue)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("https://www.google.com/maps/search/?api=1&query=");
            builder.Append(venue.Lat.ToString() + "," + venue.Lon.ToString());

            return builder.ToString();
        }

        #endregion
    }
}
