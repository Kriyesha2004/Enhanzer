using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Enhanzer.API.DTOs
{
    public class CreatePurchaseOrderRequest
    {
        public DateTime? OrderDate { get; set; }
        public decimal? NetAmount { get; set; }
        public List<PurchaseOrderItemInputDto> Items { get; set; } = new();
    }

    public class PurchaseOrderItemInputDto
    {
        [JsonPropertyName("item")]
        public string? Item { get; set; }

        // think this might be a problem
        [JsonPropertyName("itemName")]
        public string? ItemName { get; set; }

        public string Batch { get; set; } = string.Empty;
        public decimal StandardCost { get; set; }
        public decimal StandardPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal FreeQty { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalSelling { get; set; }

        public string ResolveItemName()
        {
            if (!string.IsNullOrWhiteSpace(ItemName))
                return ItemName.Trim();
            if (!string.IsNullOrWhiteSpace(Item))
                return Item.Trim();
            return string.Empty;
        }
    }

    public class PurchaseOrderResponseDto
    {
        public int PurchaseOrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal NetAmount { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PurchaseOrderItemResponseDto> Items { get; set; } = new();
    }

    public class PurchaseOrderItemResponseDto
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
    }
}

