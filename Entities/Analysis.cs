using System;

namespace Cor.Apt.Entities
{
    public class Analysis
    {
        public int AnalysisId { get; set; }
        public DateTime AnalysisDate { get; set; }
        public string Description { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}