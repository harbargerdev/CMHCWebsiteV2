using System;
using System.Collections.Generic;
using System.Text;

namespace CMHCWebsite.Library.ContentManager.Entities
{
    [Serializable]
    public class StaffEntity
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Bio { get; set; }
        public string ImgUrl { get; set; }
    }
}
