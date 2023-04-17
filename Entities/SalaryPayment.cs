using System;

namespace Cor.Apt.Entities
{
    public class SalaryPayment
    {
        public int SalaryPaymentId { get; set; }
        public DateTime RecordDate { get; set; }
        public Double Amount { get; set; }

        public int SalaryPaymentTypeId { get; set; }
        public SalaryPaymentType SalaryPaymentType { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}