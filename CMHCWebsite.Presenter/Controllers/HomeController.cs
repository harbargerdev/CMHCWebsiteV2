﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CMHCWebsite.Presenter.Models;
using CMHCWebsite.Library.MeetupAPI;
using CMHCWebsite.Library.MeetupAPI.Entities;

namespace CMHCWebsite.Presenter.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AboutUS(string viewMode = null)
        {
            if (viewMode == null)
                viewMode = "history";

            ViewData["ActiveView"] = viewMode;

            return View();
        }

        #region Helper Methods

        private string BuildEventsTable(List<MeetupEventEntity> events)
        {
            string html = string.Empty;

            if(events.Count > 0)
            {
                html = "<table id=\"upcomingEventsTable\" style=\"padding: 1px;width: 99%;\">";
                html += "<col width=\"4\"><col width=\"4\"><col width=\"75\"><col width=\"6\"><col width=\"6\">" +
                    "<col width=\"30\"><col width=\"15\"";
                html += "<tr style=\"font-weight: bold;\"><th>Date</th><th>Time</th><th>Event Title</th>" +
                    "<th>RSVPs</th><th>Wait List</th><th>Location Name</th><th></th></tr>";

                foreach(MeetupEventEntity mEvent in events)
                {
                    html += "<tr>";
                    // Event Date
                    html += "<td>" + mEvent.LocalDate + "</td>";
                    // Event Time
                    html += "<td>" + mEvent.LocalTime + "</td>";
                    // Event Title
                    html += "<td>" + mEvent.Name + "</td>";
                    // RSVPs
                    html += "<td>" + mEvent.YesRSVPCount + "</td>";
                    // Wait List
                    string hasWaitList = mEvent.WaitlistCount > 0 ? "Y" : "N";
                    html += "<td>" + hasWaitList + "</td>";
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

        #endregion
    }
}
