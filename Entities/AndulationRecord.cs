using System;

namespace Cor.Apt.Entities
{
    public class AndulationRecord
    {
        public int AndulationRecordId { get; set; }
        public DateTime SessionDate { get; set; }
        public int Session { get; set; }
        public double Chest { get; set; }
        public double Waist { get; set; }
        public double Abdomen { get; set; }
        public double Hip { get; set; }
        public double RightLeg { get; set; }
        public double RightArm { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}