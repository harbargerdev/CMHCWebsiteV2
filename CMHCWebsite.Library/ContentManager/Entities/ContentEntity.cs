using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CMHCWebsite.Library.ContentManager.Entities
{
    public enum ContentSource { DynamoDb = 0, S3 }

    [Serializable]
    public class ContentEntity
    {
        public DateTime LastUpdDt { get; set; }
        [JsonProperty("ContentKey")]
        public string ContentKey { get; set; }
        [JsonProperty("Content")]
        public string ContentHtml { get; set; }
        [JsonProperty("Filename")]
        public string Filename { get; set; }
        public ContentSource Source { get; set; }

        public ContentEntity() { }
    }
}