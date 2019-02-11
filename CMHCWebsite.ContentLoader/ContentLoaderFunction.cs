using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;

using CMHCWebsite.Library.ContentManager;
using CMHCWebsite.Library.ContentManager.Entities;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace CMHCWebsite.ContentLoader
{
    public class ContentLoaderFunction
    {
        private const string BUCKET_NAME = "cmhc-content-loader";
        private const string IMPORT_BUCKET_NAME = "cmhc-imported";
        IAmazonS3 S3Client { get; set; }

        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public ContentLoaderFunction()
        {
            S3Client = new AmazonS3Client();
        }


        public ContentLoaderFunction(IAmazonS3 s3Client)
        {
            this.S3Client = s3Client;
        }
        
        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an S3 event object and can be used 
        /// to respond to S3 notifications.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> S3EventHandler(S3Event evnt, ILambdaContext context)
        {
            LambdaLogger.Log("Event triggered, starting S3EventHandler. Event: " + JsonConvert.SerializeObject(evnt));

            var s3Event = evnt.Records?[0].S3;
            if(s3Event == null)
            {
                return null;
            }



            try
            {
                string responsebody = "";

                GetObjectRequest getRequest = new GetObjectRequest()
                {
                    BucketName = BUCKET_NAME,
                    Key = evnt.Records.FirstOrDefault().S3.Object.Key
                };

                using (GetObjectResponse getResponse = await S3Client.GetObjectAsync(getRequest))
                using (Stream responseStream = getResponse.ResponseStream)
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    responsebody = reader.ReadToEnd();
                    LambdaLogger.Log("Retrieved object from S3 and read. Body: " + responsebody);
                }

                ContentEntity content = new ContentEntity()
                {
                    ContentKey = getRequest.Key,
                    ContentHtml = responsebody
                };

                LambdaLogger.Log("Calling Update content. Content: " + JsonConvert.SerializeObject(content));
                ContentUtility utility = new ContentUtility();
                bool result = utility.UpdateContent(content);

                CopyObjectRequest copyRequest = new CopyObjectRequest
                {
                    SourceBucket = BUCKET_NAME,
                    SourceKey = evnt.Records.FirstOrDefault().S3.Object.Key,
                    DestinationBucket = IMPORT_BUCKET_NAME,
                    DestinationKey = evnt.Records.FirstOrDefault().S3.Object.Key
                };

                LambdaLogger.Log("Copying to migrated bucket. Copy Request: " + JsonConvert.SerializeObject(copyRequest));
                var copyResponse = S3Client.CopyObjectAsync(copyRequest);
                LambdaLogger.Log("Copy executed. Copy Request: " + JsonConvert.SerializeObject(copyResponse));

                DeleteObjectRequest delRequest = new DeleteObjectRequest
                {
                    BucketName = BUCKET_NAME,
                    Key = evnt.Records.FirstOrDefault().S3.Object.Key
                };

                LambdaLogger.Log("Delete request created, calling now ... Request: " + JsonConvert.SerializeObject(delRequest));
                var delResponse = S3Client.DeleteObjectAsync(delRequest);
                LambdaLogger.Log("Delete request processed. Response: " + JsonConvert.SerializeObject(delResponse));

                return result ? "Content was updated successfully." : "Content failed to update, please check the error logs.";
            }
            catch(Exception e)
            {
                context.Logger.LogLine($"Error getting object {s3Event.Object.Key} from bucket {s3Event.Bucket.Name}. Make sure they exist and your bucket is in the same region as this function.");
                context.Logger.LogLine(e.Message);
                context.Logger.LogLine(e.StackTrace);
                return "Error occured, please check the logs.";
            }
        }

        private void LogTracer(string step, object obj)
        {
            string json = string.Empty;

            if (obj != null)
                json = JsonConvert.SerializeObject(obj);

            LambdaLogger.Log(String.Format("Step {0} starting, object definition: {1}", step, json));
        }
    }
}
