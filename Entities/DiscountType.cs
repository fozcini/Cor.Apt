using System;
using System.Collections.Generic;

namespace Cor.Apt.Entities
{
    public class DiscountType
    {
        public int DiscountTypeId { get; set; }
        public string DiscountTypeName { get; set; }
        public Double Percentage { get; set; }

        public ICollection<Patient> Patients { get; set; }
    }
}