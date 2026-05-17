using System.Collections.Generic;

namespace WarehouseData.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public bool Outgoing { get; set; } 
        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    }

    public class InvoiceItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}