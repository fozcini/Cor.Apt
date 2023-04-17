using System;

namespace Cor.Apt.Entities
{
    public class PatientRecord
    {
        public int PatientRecordId { get; set; }
        public DateTime RecordDate { get; set; }
        public string Description { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}