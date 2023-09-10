using System;

namespace Cor.Apt.Entities
{
    public class StockRecord
    {
        public int StockRecordId { get; set; }
        public string InvoiceId { get; set; }
        public string InvoiceDetails { get; set; }
        public DateTime RecordDate { get; set; }
        public int Piece { get; set; }
        public double UnitAmount { get; set; }
        public double TotalAmount { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}