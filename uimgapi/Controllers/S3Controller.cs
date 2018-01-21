using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;
using uimgapi.Models;

namespace uimgapi.Controllers
{
    [Route("aws/s3")]
    public class S3Controller : Controller
    {
        IAmazonS3 S3Client { get; set; }
        S3Objects S3Items;

        public S3Controller(IAmazonS3 s3Client)
        {
            this.S3Client = s3Client;
            this.S3Items = new S3Objects(S3Client);
        }

        // GET: aws/s3
        [HttpGet]
        public async Task<List<S3Object>> GetAwsS3()
        {
            return await S3Items.listS3Object();
        }

        //POST 
        [HttpPost]
        public void UploadS3()
        {
            int count = 1;
            //S3Items.uploadS3Object("C:\\Users\\YamakasiPendragon\\Pictures\\1544941.jpg", "ImageUploadTest", "Testing_" + count);
        }

    }

}