using System;

namespace Cor.Apt.Entities
{
    public class ConsentForm
    {
        public int ConsentFormId { get; set; }
        public DateTime RecordDate { get; set; }
        public string ConsentFormName { get; set; }
        public string Details { get; set; }
    }
}