using System;

namespace Cor.Apt.Entities
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string FullName { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ReferenceNumber { get; set; }
    }
}