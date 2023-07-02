using System;

namespace Cor.Apt.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string Phone { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        
        public int AppointmentTypeId { get; set; }
        public AppointmentType AppointmentType { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}