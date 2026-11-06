using System;
using System.Collections.Generic;

namespace Enhanzer.API.Models
{
    public class PurchaseOrder
    {
        public int PurchaseOrderId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal NetAmount { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalQuantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
    }
}

