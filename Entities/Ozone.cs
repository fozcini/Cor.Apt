using System;

namespace Cor.Apt.Entities
{
    public class Ozone
    {
        public int OzoneId { get; set; }
        public DateTime SessionDate { get; set; }
        public int Session { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}