using System;

namespace Cor.Apt.Entities
{
    public class Leave
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Days {get;set;}
        
        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}