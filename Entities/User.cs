using System;
using System.Text.Json.Serialization;

namespace Cor.Apt.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string IdentificationNumber { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public string SecondPhone { get; set; }
        public string Email { get; set; }
        public string RegistrationNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string PhotoLink { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}