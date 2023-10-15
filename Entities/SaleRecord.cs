using System;

namespace Cor.Apt.Entities
{
    public class SaleRecord
    {
        public int SaleRecordId { get; set; }
        public DateTime SaleRecordDate { get; set; }
        public string Description { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}