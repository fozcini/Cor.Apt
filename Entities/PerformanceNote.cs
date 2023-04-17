using System;

namespace Cor.Apt.Entities
{
    public class PerformanceNote
    {
        public int PerformanceNoteId { get; set; }
        public DateTime RecordDate { get; set; }
        public string Description { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}