using System;

namespace Cor.Apt.Entities
{
    public class StockRecordDecrease
    {
        public int StockRecordDecreaseId { get; set; }
        public string Details { get; set; }
        public DateTime RecordDate { get; set; }
        public int Piece { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}