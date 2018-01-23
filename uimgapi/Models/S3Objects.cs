using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace uimgapi.Models
{
    public class S3Objects
    {
        IAmazonS3 s3Client;
        string bucketName = "ujets";


        public S3Objects(IAmazonS3 client)
        {
            s3Client = client;
        }

        public async Task<List<S3Object>> listS3Object()
        {
            ListObjectsResponse response2 = await s3Client.ListObjectsAsync(bucketName, new System.Threading.CancellationToken());
            if (response2.S3Objects != null &&
                response2.S3Objects.Count > 0)
            {
                return response2.S3Objects;
            }
            else
            {
                return null;
            }

        }

        public string uploadS3Object(string fileb64Str,string keyName)
        {
            try
            {
                //TransferUtility fileTransferUtility = new
                //        TransferUtility(s3Client);

                TransferUtility fileTransferUtility = new
                        TransferUtility();

                var bytes = Convert.FromBase64String(fileb64Str);
                var contents = new MemoryStream(bytes);
                fileTransferUtility.Upload(contents, bucketName, keyName);
                string link = string.Format(@"https://{0}.s3.us-east-2.amazonaws.com/{1}", bucketName,keyName);
                return link;
            }

            catch (AmazonS3Exception s3Exception)
            {
                Console.WriteLine(s3Exception.Message,
                                  s3Exception.InnerException);
                return null;
            }
        }

        public void deleteS3Oject()
        {

        }

        public void modifiedS3Ojbect()
        {

        }

        public void uploadSameple()
        {
            //TransferUtility fileTransferUtility = new
            //            TransferUtility(s3Client);

            //// 1. Upload a file, file name is used as the object key name.
            //fileTransferUtility.Upload(filePath, existingBucketName);
            //Console.WriteLine("Upload 1 completed");

            // 2. Specify object key name explicitly.
            //fileTransferUtility.Upload(filePath,
            //                          existingBucketName, keyName);
            //Console.WriteLine("Upload 2 completed");

            //// 3. Upload data from a type of System.IO.Stream.
            //using (FileStream fileToUpload =
            //    new FileStream(filePath, FileMode.Open, FileAccess.Read))
            //{
            //    fileTransferUtility.Upload(fileToUpload,
            //                               existingBucketName, keyName);
            //}
            //Console.WriteLine("Upload 3 completed");

            //// 4.Specify advanced settings/options.
            //TransferUtilityUploadRequest fileTransferUtilityRequest = new TransferUtilityUploadRequest
            //{
            //    BucketName = existingBucketName,
            //    FilePath = filePath,
            //    StorageClass = S3StorageClass.ReducedRedundancy,
            //    PartSize = 6291456, // 6 MB.
            //    Key = keyName,
            //    CannedACL = S3CannedACL.PublicRead
            //};
            //fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
            //fileTransferUtilityRequest.Metadata.Add("param2", "Value2");
            //fileTransferUtility.Upload(fileTransferUtilityRequest);
            //Console.WriteLine("Upload 4 completed");
        }

    }
}
