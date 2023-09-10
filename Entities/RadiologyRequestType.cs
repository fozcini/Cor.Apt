using System;

namespace Cor.Apt.Entities
{
    public class RadiologyRequestType
    {
        public int RadiologyRequestTypeId { get; set; }
        public double Price { get; set; }
        public Boolean IsDone { get; set; }
        public string IsEndoklinikTest { get; set; }

        public int RadiologyRequestId { get; set; }
        public RadiologyRequest RadiologyRequest { get; set; }
        public int RadiologyRequestTypeListId { get; set; }
        public RadiologyRequestTypeList RadiologyRequestTypeList { get; set; }
    }
}