using System;

namespace Cor.Apt.Entities
{
    public class PhysicalExamination
    {
        public int PhysicalExaminationId { get; set; }
        public DateTime RecordDate { get; set; }
        public string BloodPressure { get; set; }
        public string Pulse { get; set; }
        public string Description { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}