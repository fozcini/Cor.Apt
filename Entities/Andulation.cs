using System;

namespace Cor.Apt.Entities
{
    public class Andulation
    {
        public int AndulationId { get; set; }
        public DateTime SessionDate { get; set; }
        public string Reason { get; set; }
        public string Description { get; set; }
        public int Session { get; set; }
        public int Schedule { get; set; }
        public int Duration { get; set; }
        public int Vibration { get; set; }
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