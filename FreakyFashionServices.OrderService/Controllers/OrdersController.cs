using FreakyFashionServices.OrderService.Data;
using FreakyFashionServices.OrderService.Models.Domain;
using FreakyFashionServices.OrderService.Models.DTO;
using FreakyFashionServices.OrderService.Models.DTO.BasketService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace FreakyFashionServices.OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderServiceContext _context;
        private readonly IHttpClientFactory httpClientFactory;

        public OrdersController(OrderServiceContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            this.httpClientFactory = httpClientFactory;
        }

        // POST /api/orders
        [HttpPost]
        public async Task<IActionResult> RegisterOrder(OrderDto orderDto)
        {
            // Fetch data from basket based on common identifier
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                $"http://localhost:8000/api/baskets/{orderDto.Identifier}")
            {
                Headers = { { HeaderNames.Accept, "application/json" }, }
            };

            var httpClient = httpClientFactory.CreateClient();

            using var httpResponseMessage =
                await httpClient.SendAsync(httpRequestMessage);


            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                return NotFound();
            }

            using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var basket = await JsonSerializer.DeserializeAsync<BasketDto>(contentStream, options);

            // Add basket items as orderlines to order
            var order = new Order(orderDto.Identifier, orderDto.Customer);
            var orderLines = basket?.Items.Select(item => new OrderLine(item.ProductId, item.Quantity));

            if (orderLines.Any())
            {
                foreach (var orderline in orderLines)
                {
                    order.OrderLines.Add(orderline);
                }
            }           

            // Add order to the database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Created("", order); // 201 Created
        }
    }
}
