using System;

namespace Cor.Apt.Entities
{
    public class Consent
    {
        public int ConsentId { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string IdentificationNumber { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string MessageId { get; set; }
        public string Log { get; set; }
        public bool IsApproved { get; set; }

        public int ConsentFormId { get; set; }
        public ConsentForm ConsentForm { get; set; }
    }
}