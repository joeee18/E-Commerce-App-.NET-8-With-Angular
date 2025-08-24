using ECom.API.Helper;
using ECom.Core.DTO;
using ECom.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpPost("create-order")]
        public async Task<IActionResult> create(OrderDTO orderDTO)
        {
            
            var email = User?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new ResponseAPI(401, "User not authenticated"));
            }
            var order = await _orderService.CreateOrdersAsync(orderDTO, email);
            if (order is null)
            {
                return BadRequest(new ResponseAPI(400, "Failed to create order"));
            }
            return Ok(order);
        }
        [HttpGet("get-orders-for-user")]
        public async Task<IActionResult> getOrders()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new ResponseAPI(401, "User not authenticated"));
            }
            var orders = await _orderService.GetAllOrdersForUserAsync(email);
            if (orders is null || !orders.Any())
            {
                return NotFound(new ResponseAPI(404, "No orders found for this user"));
            }
            return Ok(orders);
        }
        [HttpGet("get-order-by-id/{id}")]
        public async Task<IActionResult> getOrderById(int id)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new ResponseAPI(401, "User not authenticated"));
            }
            var order = await _orderService.GetOrderByIdAsync(id, email);
            if (order is null)
            {
                return NotFound(new ResponseAPI(404, "Order not found"));
            }
            return Ok(order);
        }
        [HttpGet("get-delivery")]
        public async Task<IActionResult> GetDeliveryMethods() 
            =>  Ok(await _orderService.GetDeliveryMethodsAsync());
        
    }
}
