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
            ViewData["Message"] = "Upcoming Meetups";

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

        public IActionResult Programs(string activeView = "EduWorkshops")
        {
            ViewData["Content"] = GetContent(activeView);
            return View();
        }

        public IActionResult AboutUs(string activeView = "")
        {

            return View();
        }

        public IActionResult CrisisHelp()
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
                html = "<table id=\"upcomingEventsTable\" class=\"table-striped\" style=\"padding: 1px;width: 99%;\">";
                html += "<tr style=\"font-weight: bold;\"><th>Event Start</th><th>Event Title</th><th>RSVPs</th>" + 
                    "<th>Location Name</th><th></th></tr>";

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
                    html += "<td>" + mEvent.Venue.Name + "</td>";
                    // Link
                    html += "<td><a href=\"" + mEvent.Url + "\">Join Now!</a></td>";

                    html += "</tr>";
                }

                html += "</table>";
            }

            return html;
        }

        private string GetContent(string key)
        {
            ContentUtility cUtility = new ContentUtility();

            return cUtility.GetContent(Library.ContentManager.Entities.ContentSource.DynamoDb, key);
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
            builder.Append("<col width =\"120\"><col width=\"120\"><col width=\"120\"><col width=\"400\"><col width=\"80\">");
            builder.Append("<tr><th>Staff Name</th><th>Type</th><th>Role</th><th>Bio</th><th>Picture</th></tr>");

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

        #endregion
    }
}
