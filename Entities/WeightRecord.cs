using System;

namespace Cor.Apt.Entities
{
    public class WeightRecord
    {
        public int WeightRecordId { get; set; }
        public DateTime RecordDate { get; set; }
        public string RecordName { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}