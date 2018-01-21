using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using uimgapi.Models;
using Amazon.S3;

namespace uimgapi.Controllers
{
    [Produces("application/json")]
    [Route("api/AwsS3")]
    public class AwsS3Controller : Controller
    {
        private readonly s3uploadtestContext _context;
        IAmazonS3 S3Client { get; set; }
        S3Objects S3Items;
        public AwsS3Controller(s3uploadtestContext context,IAmazonS3 client)
        {
            _context = context;
            S3Client = client;
            S3Items = new S3Objects(S3Client);
        }

        // GET: api/AwsS3
        [HttpGet]
        public IEnumerable<AwsS3> GetAwsS3(string searchString)
        {
            var images = from image in _context.AwsS3
                         select image;

            if (!String.IsNullOrEmpty(searchString))
            {

                images = images.Where(image => image.UniqueCode.Contains(searchString)
                                                || image.Name.Contains(searchString)
                                                || image.Filename.Contains(searchString)
                                                || image.ApprovalStatus.Contains(searchString)
                                                || image.EmailStatus.Contains(searchString));
            }

            return images;
        }

        // GET: api/AwsS3/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAwsS3([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var awsS3 = await _context.AwsS3.SingleOrDefaultAsync(m => m.Id == id);

            if (awsS3 == null)
            {
                return NotFound();
            }

            return Ok(awsS3);
        }



        // PUT: api/AwsS3/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAwsS3([FromRoute] long id, [FromBody] AwsS3 awsS3)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != awsS3.Id)
            {
                return BadRequest();
            }

            _context.Entry(awsS3).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AwsS3Exists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AwsS3
        [HttpPost]
        public async Task<IActionResult> PostAwsS3([FromBody] dynamic data)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Parse Data
            Newtonsoft.Json.Linq.JToken[] dataArray = ((Newtonsoft.Json.Linq.JContainer)data).ToArray();
            string imageData = dataArray[0].First.ToString();
            string unquieID = dataArray[1].First.ToString();
            string name = dataArray[2].First.ToString();

            string link = S3Items.uploadS3Object(imageData, unquieID);

            if (link != null)
            {
                AwsS3 awsS3 = new AwsS3()
                {
                    UniqueCode = unquieID,
                    Name = name,
                    Filename = "",
                    EmailStatus = "Sent",
                    UploadedDate = DateTime.Now.ToShortDateString(),
                    ImageLink = link,
                    ApprovalStatus = "Pending",
                    ApprovalDecisionNotes = ""
                };

                _context.AwsS3.Add(awsS3);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (AwsS3Exists(awsS3.Id))
                    {
                        return new StatusCodeResult(StatusCodes.Status409Conflict);
                    }
                    else
                    {
                        throw;
                    }
                }
                return CreatedAtAction("GetAwsS3", new { id = awsS3.Id }, awsS3);
            }
            return BadRequest();
        }

        // DELETE: api/AwsS3/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAwsS3([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var awsS3 = await _context.AwsS3.SingleOrDefaultAsync(m => m.Id == id);
            if (awsS3 == null)
            {
                return NotFound();
            }

            _context.AwsS3.Remove(awsS3);
            await _context.SaveChangesAsync();

            return Ok(awsS3);
        }

        private bool AwsS3Exists(long id)
        {
            return _context.AwsS3.Any(e => e.Id == id);
        }
    }
}