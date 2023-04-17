using System;

namespace Cor.Apt.Entities
{
    public class Radiology
    {
        public int RadiologyId { get; set; }
        public DateTime RecordDate { get; set; }
        public string Description { get; set; }

        public int RadiologyTypeId { get; set; }
        public RadiologyType RadiologyType { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}