using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.S3;
using CMHCWebsite.Library.ContentManager.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace CMHCWebsite.Library.ContentManager
{
    public class ContentUtility
    {
        private string DEFAULT_REGION = "us-east-1";
        private string TABLE_NAME = "CMHC.ContentSite.Content";

        public ContentUtility() { }

        public string GetContent(ContentSource source, string key)
        {
            return source == ContentSource.DynamoDb ? GetContentFromDynamoDb(key) : GetContentFromFile(key);
        }

        private string GetContentFromDynamoDb(string key)
        {
            string content = string.Empty;

            try
            {
                AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
                clientConfig.RegionEndpoint = RegionEndpoint.USEast1;

                AmazonDynamoDBClient client = new AmazonDynamoDBClient(clientConfig);

                GetItemRequest request = new GetItemRequest();
                request.TableName = TABLE_NAME;
                request.Key = new Dictionary<string, AttributeValue>();
                request.Key.Add("ContentKey", new AttributeValue() { S = key });

                var response = client.GetItemAsync(request);

                if(response.Result.HttpStatusCode == HttpStatusCode.OK)
                {
                    var item = response.Result.Item;
                    content = item["Content"].S;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return content;
        }

        private string GetContentFromFile(string key)
        {
            return string.Empty;
        }
    }
}
