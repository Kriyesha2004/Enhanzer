namespace Enhanzer.API.Models
{
    public class PurchaseOrderItem
    {
        public int PurchaseOrderItemId { get; set; }
        public int PurchaseOrderId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string Batch { get; set; } = string.Empty;
        public decimal StandardCost { get; set; }
        public decimal StandardPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal FreeQty { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalSelling { get; set; }

        public PurchaseOrder? PurchaseOrder { get; set; }
    }
}

