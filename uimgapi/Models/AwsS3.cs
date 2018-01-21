using System;
using System.Collections.Generic;

namespace uimgapi.Models
{
    public partial class AwsS3
    {
        public long Id { get; set; }
        public string UniqueCode { get; set; }
        public string Name { get; set; }
        public string Filename { get; set; }
        public string EmailStatus { get; set; }
        public string UploadedDate { get; set; }
        public string ImageLink { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovalDecisionNotes { get; set; }
    }
}
