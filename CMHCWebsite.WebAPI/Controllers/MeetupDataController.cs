using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMHCWebsite.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetupDataController : ControllerBase
    {
        public IActionResult GetEvents()
        {
            JsonResult response = null;

            return response;
        }
    }
}