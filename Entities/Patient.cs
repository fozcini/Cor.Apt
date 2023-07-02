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
        public string SecondPhone { get; set; }
        public string ThirdPhone { get; set; }
        public string Address { get; set; }
        public string Reference { get; set; }
        public string OzoneDescription { get; set; }
        public string AndulationDescription { get; set; }
        public string CrylineDescription { get; set; }

        public int SocialSecurityId { get; set; }
        public SocialSecurity SocialSecurity { get; set; }
        public int DiscountTypeId { get; set; }
        public DiscountType DiscountType { get; set; }
    }
}