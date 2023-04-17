using System;

namespace Cor.Apt.Entities
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public string Cv { get; set; }
        public string PhotoLink { get; set; }

        public int ApplicationTypeId { get; set; }
        public ApplicationType ApplicationType { get; set; }
    }
}