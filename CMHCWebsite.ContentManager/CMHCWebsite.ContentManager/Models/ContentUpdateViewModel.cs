using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMHCWebsite.ContentManager.Models
{
    public enum ContentTypes
    {
        HomePage = 0,
        Mission,
        History,
        CrisisHelp,
        Meetups,
        CommEducation,
        volunteer,
        advocacy,
        localResources,
        onlineResources
    }

    public class ContentUpdateViewModel
    {
        public ContentTypes ContentType { get; set; }
        public string Filename { get; set; }
        public string OriginalContent { get; set; }
        public string UpdatedContent { get; set; }
    }
}
