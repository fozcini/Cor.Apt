using System;

namespace Cor.Apt.Entities
{
    public class Cryline
    {
        public int CrylineId { get; set; }
        public DateTime RecordDate { get; set; }
        public string Area { get; set; }
        public int Seans { get; set; }
        public string MeasureOnFoot { get; set; }
        public string MeasureOnBed { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}