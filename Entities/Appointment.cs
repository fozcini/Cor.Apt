using System;

namespace Cor.Apt.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public double Fee { get; set; }
        public Boolean IsPaid { get; set; }
        public Boolean IsAndulation { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}