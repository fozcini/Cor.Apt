using System;

namespace Cor.Apt.Entities
{
    public class DiagnosisRecord
    {
        public int DiagnosisRecordId { get; set; }
        public DateTime RecordDate { get; set; }
        public string Description { get; set; }

        public int DiagnosisId { get; set; }
        public Diagnosis Diagnosis { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}