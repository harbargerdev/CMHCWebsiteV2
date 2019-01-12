using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CMHCWebsite.Library.ContentManager.Entities
{
    public enum STAFF_TYPE
    {
        All,
        [Description("Full-time Staff")]
        FullTimeStaff = 1,
        [Description("Part-time Staff")]
        PartTimeStaff = 2,
        [Description("Volunteer")]
        Volunteer = 3
    }

    [Serializable]
    public class StaffEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Bio { get; set; }
        public STAFF_TYPE Category { get; set; }
        public bool IsActive { get; set; }
        public string ImgUrl { get; set; }
    }
}
