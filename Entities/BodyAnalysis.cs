using System;

namespace Cor.Apt.Entities
{
    public class BodyAnalysis
    {
        public int BodyAnalysisId { get; set; }
        public DateTime RecordDate { get; set; }
        public Double Height { get; set; }
        public Double Weight { get; set; }
        public Double Bmi { get; set; }
        public Double Fat { get; set; }
        public Double Fm { get; set; }
        public Double Mm { get; set; }
        public Double Tbw { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}