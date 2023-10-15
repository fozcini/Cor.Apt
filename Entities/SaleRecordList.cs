using System;

namespace Cor.Apt.Entities
{
    public class SaleRecordList
    {
        public int SaleRecordListId { get; set; }
        public int Piece { get; set; }
        public double Price { get; set; }
        public Boolean IsDone { get; set; }
        public string IsEndoklinikTest { get; set; }

        public int SaleRecordId { get; set; }
        public SaleRecord SaleRecord { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}