using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using OrderService.Repository;
using System.Net.Http.Json;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly OrderRepository _repository;
        private readonly HttpClient _httpClient;

        public OrderController(OrderRepository repository)
        {
            _repository = repository;
            _httpClient = new HttpClient(); // works for now
        }

        // 1️⃣ Create Order
        [HttpPost("create")]
        public IActionResult CreateOrder(Order order)
        {
            order.Status = "Created";
            int orderId = _repository.CreateOrder(order);
            order.Id = orderId;

            return Ok(order);
        }

        // 2️⃣ Place Order (Inventory + Payment)
        [HttpPost("place/{orderId}")]
        public async Task<IActionResult> PlaceOrder(int orderId)
        {
            // ✅ Read order from DB
            var order = _repository.GetOrderById(orderId);
            if (order == null)
                return NotFound("Order not found");

            // ✅ Inventory Service
            var inventoryResponse = await _httpClient.PostAsJsonAsync(
                "http://localhost:5003/api/inventory/reserve",
                new { order.ProductName, order.Quantity });

            if (!inventoryResponse.IsSuccessStatusCode)
            {
                _repository.UpdateStatus(orderId, "InventoryFailed");
                return BadRequest("Inventory reservation failed");
            }

            // ✅ Payment Service
            var paymentResponse = await _httpClient.PostAsJsonAsync(
                "http://localhost:5002/api/payment/pay",
                new { Id = orderId, Amount = order.Amount });

            if (!paymentResponse.IsSuccessStatusCode)
            {
                _repository.UpdateStatus(orderId, "PaymentFailed");
                return BadRequest("Payment failed");
            }

            // ✅ Final status
            _repository.UpdateStatus(orderId, "Completed");

            return Ok(new
            {
                message = "Order placed successfully",
                orderId,
                status = "Completed"
            });
        }
    }
}