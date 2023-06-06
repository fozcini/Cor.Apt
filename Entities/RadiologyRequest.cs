using System;

namespace Cor.Apt.Entities
{
    public class RadiologyRequest
    {
        public int RadiologyRequestId { get; set; }
        public DateTime RadiologyRequestDate { get; set; }
        public string Description { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}