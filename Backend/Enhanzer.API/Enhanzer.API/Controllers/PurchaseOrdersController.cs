using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enhanzer.API.Data;
using Enhanzer.API.DTOs;
using Enhanzer.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enhanzer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PurchaseOrdersController> _logger;

        public PurchaseOrdersController(
            ApplicationDbContext context,
            ILogger<PurchaseOrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST: api/PurchaseOrders
        [HttpPost]
        public async Task<IActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderRequest request)
        {
            if (request == null || request.Items == null || request.Items.Count == 0)
            {
                return BadRequest(new { message = "At least one purchase order item is required." });
            }

            foreach (var item in request.Items)
            {
                var resolvedName = item.ResolveItemName();
                if (string.IsNullOrWhiteSpace(resolvedName))
                {
                    return BadRequest(new { message = "Item name is required for all items." });
                }

                if (string.IsNullOrWhiteSpace(item.Batch))
                {
                    return BadRequest(new { message = $"Batch / location is required for item '{resolvedName}'." });
                }

                if (item.Quantity <= 0)
                {
                    return BadRequest(new { message = $"Quantity must be greater than zero for item '{resolvedName}'." });
                }
            }

            try
            {
                var order = new PurchaseOrder
                {
                    OrderDate = request.OrderDate ?? DateTime.UtcNow,
                    TotalItems = request.Items.Count,
                    TotalQuantity = request.Items.Sum(i => i.Quantity),
                    NetAmount = request.NetAmount ?? request.Items.Sum(i => i.TotalCost),
                    CreatedAt = DateTime.UtcNow
                };

                foreach (var item in request.Items)
                {
                    order.Items.Add(new PurchaseOrderItem
                    {
                        ItemName = item.ResolveItemName(),
                        Batch = item.Batch.Trim(),
                        StandardCost = item.StandardCost,
                        StandardPrice = item.StandardPrice,
                        Quantity = item.Quantity,
                        FreeQty = item.FreeQty,
                        Discount = item.Discount,
                        TotalCost = item.TotalCost,
                        TotalSelling = item.TotalSelling
                    });
                }

                _context.PurchaseOrders.Add(order);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully created purchase order ID {OrderId} with {ItemCount} items.", order.PurchaseOrderId, order.Items.Count);

                var responseDto = MapToResponseDto(order);

                return CreatedAtAction(nameof(GetPurchaseOrderById), new { id = order.PurchaseOrderId }, responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the purchase order.");
                return StatusCode(500, new { message = "An error occurred while saving the purchase order." });
            }
        }

        // GET: api/PurchaseOrders
        [HttpGet]
        public async Task<IActionResult> GetAllPurchaseOrders()
        {
            try
            {
                var orders = await _context.PurchaseOrders
                    .Include(p => p.Items)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();

                var response = orders.Select(MapToResponseDto).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching purchase orders.");
                return StatusCode(500, new { message = "An error occurred while fetching purchase orders." });
            }
        }

        // GET: api/PurchaseOrders/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseOrderById(int id)
        {
            try
            {
                var order = await _context.PurchaseOrders
                    .Include(p => p.Items)
                    .FirstOrDefaultAsync(p => p.PurchaseOrderId == id);

                if (order == null)
                {
                    return NotFound(new { message = $"Purchase order with ID {id} was not found." });
                }

                return Ok(MapToResponseDto(order));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching purchase order {Id}.", id);
                return StatusCode(500, new { message = "An error occurred while fetching the purchase order." });
            }
        }

        private static PurchaseOrderResponseDto MapToResponseDto(PurchaseOrder order)
        {
            return new PurchaseOrderResponseDto
            {
                PurchaseOrderId = order.PurchaseOrderId,
                OrderDate = order.OrderDate,
                NetAmount = order.NetAmount,
                TotalItems = order.TotalItems,
                TotalQuantity = order.TotalQuantity,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(item => new PurchaseOrderItemResponseDto
                {
                    PurchaseOrderItemId = item.PurchaseOrderItemId,
                    PurchaseOrderId = item.PurchaseOrderId,
                    ItemName = item.ItemName,
                    Batch = item.Batch,
                    StandardCost = item.StandardCost,
                    StandardPrice = item.StandardPrice,
                    Quantity = item.Quantity,
                    FreeQty = item.FreeQty,
                    Discount = item.Discount,
                    TotalCost = item.TotalCost,
                    TotalSelling = item.TotalSelling
                }).ToList()
            };
        }
    }
}

