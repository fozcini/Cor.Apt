using System;

namespace Cor.Apt.Entities
{
    public class Andulation
    {
        public int AndulationId { get; set; }
        public DateTime RecordDate { get; set; }
        public int Session { get; set; }
        public string Program { get; set; }
        public bool IsDone { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}