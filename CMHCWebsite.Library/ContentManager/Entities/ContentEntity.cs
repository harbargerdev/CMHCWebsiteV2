using System;
using System.Collections.Generic;
using System.Text;

namespace CMHCWebsite.Library.ContentManager.Entities
{
    public enum ContentSource { DynamoDb = 0, S3 }

    [Serializable]
    public class ContentEntity
    {
        public DateTime LastUpdDt { get; set; }
        public string ContentKey { get; set; }
        public string ContentHtml { get; set; }
        public ContentSource Source { get; set; }

        public ContentEntity() { }
    }
}