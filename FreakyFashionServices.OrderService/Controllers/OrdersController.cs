using FreakyFashionServices.OrderService.Data;
using FreakyFashionServices.OrderService.Models.Domain;
using FreakyFashionServices.OrderService.Models.DTO;
using FreakyFashionServices.OrderService.Models.DTO.BasketService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using RabbitMQ.Client;
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
            var basket = await System.Text.Json.JsonSerializer.DeserializeAsync<BasketDto>(contentStream, options);

            
            // Instantiate a new order & retrieve respective data for its datafields
            var order = new Order(orderDto.Identifier, orderDto.Customer);
            
            var orderKey = Guid.NewGuid().ToString();
            order.OrderKey = orderKey;
            
            var orderLines = basket?.Items.Select(item => new OrderLine(item.ProductId, item.Quantity));

            if (orderLines.Any())
            {
                foreach (var orderline in orderLines)
                {
                    order.OrderLines.Add(orderline);
                }
            }
             
             
            // Construct a fresh instance of connection factory
            // with a specified connection string with a specified port
            // that is mapped for container running RabbitMq
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            using var connection = factory.CreateConnection();


            // Channel that let us talk to RabbitMQ. 
            using var channel = connection.CreateModel();

            // Säkerställ att kö finns - om inte, skapa den.
            channel.QueueDeclare(
               // Namnet på kön
               queue: "orders",
               // Vi vill att kön ska finnas kvar även om RabbitMQ startas om
               durable: true,
               // Kön får användas av flera connections.
               exclusive: false,
               // Kön ska inte automatiskt raderas så fort ingen längre finns några subscribers.
               autoDelete: false,
               // Använs av plugins och broker-specifika features, såsom message TTL
               // (time to live), queue length limit, etc.
               arguments: null);

            var body = Encoding.UTF8
               .GetBytes(JsonConvert.SerializeObject(order));

            channel.BasicPublish(
               // Det finns olika sätt att routa meddelanden - olika "exchanges".
               // Om vi anger tom textsträng får vi en "default"-exchang - när ett meddelande 
               // skickas kommer routing key avgöra vilken kö meddelandet ska till.
               exchange: "",
               // Används av exchange för att avgöra vilken kö meddelandet ska till,
               // eftersom vi använder en default exchange.
               routingKey: "orders",
               basicProperties: null,
               body: body);

            
            var orderIdObject = new OrderIdDto { OrderId = orderKey };
            
            return Accepted(orderIdObject); // 202 Accepted
        }
    }
}
