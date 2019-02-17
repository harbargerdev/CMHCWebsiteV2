using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CMHCWebsite.ContentManager.Models;
using CMHCWebsite.Library.ContentManager;

namespace CMHCWebsite.ContentManager.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ContentUpdateViewModel contentUpdateView = new ContentUpdateViewModel();

            return View(contentUpdateView);
        }

        [HttpPost]
        public ActionResult Index(ContentUpdateViewModel contentUpdateView)
        {


            return View(contentUpdateView);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetContentFromS3(string key)
        {
            ContentUtility cUtility = new ContentUtility();

            string originalContent = cUtility.GetContent(Library.ContentManager.Entities.ContentSource.S3, key);

            return null;
        }
    }
}
