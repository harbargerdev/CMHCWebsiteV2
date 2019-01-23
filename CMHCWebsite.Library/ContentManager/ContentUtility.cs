using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.S3;
using CMHCWebsite.Library.ContentManager.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Data.SqlClient;
using System.Data;
using Amazon.DynamoDBv2.DocumentModel;

namespace CMHCWebsite.Library.ContentManager
{
    public class ContentUtility
    {
        private string DEFAULT_REGION = "us-east-1";
        private string TABLE_NAME = "CMHC.ContentSite.Content";
        private string IMG_URL_PREFIX = "http://cmhc-test-staff-images.s3-website-us-east-1.amazonaws.com/";
        private string STAFF_DB_CONN_STRING = "";

        public ContentUtility() { }

        public string GetContent(ContentSource source, string key)
        {
            return source == ContentSource.DynamoDb ? GetContentFromDynamoDb(key) : GetContentFromFile(key);
        }

        public List<string> GetKeys()
        {
            List<string> keys = new List<string>();

            List<ContentEntity> fullContent = ScanTable();
            foreach(ContentEntity content in fullContent)
            {
                keys.Add(content.ContentKey);
            }

            return keys;
        }

        public List<StaffEntity> GetStaff(STAFF_TYPE sType)
        {
            List<StaffEntity> staff = new List<StaffEntity>();

            SqlConnection conn = new SqlConnection(STAFF_DB_CONN_STRING);
            SqlCommand command = new SqlCommand("GetActiveStaffByType", conn) { CommandType = CommandType.StoredProcedure };
            command.Parameters.Clear();
            int typeCode = ConvertStaffTypeToInt(sType);
            if(typeCode > 0)
                command.Parameters.Add(new SqlParameter("@staffType", typeCode));

            try
            {
                conn.Open();
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StaffEntity member = new StaffEntity();
                        member.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        member.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                        member.Role = reader.GetString(reader.GetOrdinal("Role"));
                        member.Bio = reader.GetString(reader.GetOrdinal("Bio"));
                        string mType = reader.GetString(reader.GetOrdinal("StaffType"));
                        member.Category = ConvertStringToStaffType(mType);
                        string imgFilename = reader.GetString(reader.GetOrdinal("ImgFileName"));
                        member.ImgUrl = IMG_URL_PREFIX + imgFilename;

                        staff.Add(member);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
            }

            return staff;
        }

        public bool UpdateContent(ContentEntity content)
        {
            AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
            clientConfig.RegionEndpoint = RegionEndpoint.USEast1;

            AmazonDynamoDBClient client = new AmazonDynamoDBClient(clientConfig);

            Dictionary<string, AttributeValue> item = new Dictionary<string, AttributeValue>();
            item["ContentKey"] = new AttributeValue() { S = content.ContentKey };
            item["Content"] = new AttributeValue() { S = "<p>" + content.ContentHtml + "</p>" };

            PutItemRequest request = new PutItemRequest()
            {
                TableName = TABLE_NAME,
                Item = item
            };

            try
            {
                var response = client.PutItemAsync(request);
                return response.Result.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region Private Methods

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

                if (response.Result.HttpStatusCode == HttpStatusCode.OK)
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

        private STAFF_TYPE ConvertStringToStaffType(string input)
        {
            switch (input)
            {
                case "Volunteer":
                    return STAFF_TYPE.Volunteer;
                case "PartTime":
                    return STAFF_TYPE.PartTimeStaff;
                default:
                    return STAFF_TYPE.FullTimeStaff;
            }
        }

        private int ConvertStaffTypeToInt(STAFF_TYPE sType)
        {
            switch(sType)
            {
                case STAFF_TYPE.FullTimeStaff:
                    return 1;
                case STAFF_TYPE.PartTimeStaff:
                    return 2;
                case STAFF_TYPE.Volunteer:
                    return 3;
                default:
                    return 0;
            }
        }

        private List<ContentEntity> ScanTable()
        {
            List<ContentEntity> content = new List<ContentEntity>();

            AmazonDynamoDBClient client = new AmazonDynamoDBClient(RegionEndpoint.USEast1);

            Table contentsTable = Table.LoadTable(client,TABLE_NAME);

            ScanOperationConfig scanConfig = new ScanOperationConfig();
            Search search = contentsTable.Scan(scanConfig);

            var scanResult = search.GetNextSetAsync();

            if(scanResult.Result.Count > 0)
            {
                foreach(var record in scanResult.Result)
                {
                    content.Add(ConvertDocumentToContentEntity(record));
                }
            }

            return content;
        }

        private ContentEntity ConvertDocumentToContentEntity(Document doc)
        {
            ContentEntity content = new ContentEntity();
            foreach(var attribute in doc.GetAttributeNames())
            {
                switch(attribute)
                {
                    case "ContentKey":
                        content.ContentKey = doc[attribute].AsString();
                        break;
                    case "Content":
                        content.ContentHtml = doc[attribute].AsString();
                        break;
                    default:
                        break;
                }
            }
            return content;
        }

        #endregion
    }
}
