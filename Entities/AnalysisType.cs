using System;

namespace Cor.Apt.Entities
{
    public class AnalysisType
    {
        public int AnalysisTypeId { get; set; }
        public double Price { get; set; }
        public Boolean IsDone { get; set; }
        public string IsEndoklinikTest { get; set; }

        public int AnalysisId { get; set; }
        public Analysis Analysis { get; set; }
        public int TypeId { get; set; }
        public Type Type { get; set; }
    }
}