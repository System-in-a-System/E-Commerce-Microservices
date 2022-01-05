using FreakyFashionServices.APIGateway.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace FreakyFashionServices.APIGateway.Controllers
{
    [Route("api")]
    [ApiController]
    public class GatewayController : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory;

        public GatewayController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        //GET /api/products
        [HttpGet("products")]
        public async Task<IActionResult> GetProductsAndStocks()
        {
            
            var productDtos = await FetchProducts();
            var stockLevelDtos = await FetchStocks();
            
            if (productDtos.Count() == 0)
                return NotFound(); // 404 Not Found

            var productAndStockDtos = new List<ProductAndStockDto>();
            
            foreach (var product in productDtos)
            {
                foreach (var stockLevel in stockLevelDtos)
                {
                    if(product.ArticleNumber.ToUpper() == stockLevel.ArticleNumber.ToUpper())
                    {
                        var productAndStockDto = new ProductAndStockDto 
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Description = product.Description,
                            ImageUrl = product.ImageUrl,
                            Price = product.Price,
                            ArticleNumber = product.ArticleNumber,
                            UrlSlug = product.UrlSlug,
                            Stock = stockLevel.Stock
                        };

                        productAndStockDtos.Add(productAndStockDto);
                    }
                }
            }

            return Ok(productAndStockDtos); // 200 OK
        }

        private async Task<IEnumerable<ProductDto>> FetchProducts()
        {
            var httpRequestMessage = new HttpRequestMessage(
               HttpMethod.Get,
               $"http://localhost:7000/api/products")
            {
                Headers = { { HeaderNames.Accept, "application/json" }, }
            };

            var httpClient = httpClientFactory.CreateClient();

            using var httpResponseMessage =
                await httpClient.SendAsync(httpRequestMessage);

            var productDtos = Enumerable.Empty<ProductDto>();

            if (!httpResponseMessage.IsSuccessStatusCode)
                return productDtos;

            using var contentStream =
                await httpResponseMessage.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            productDtos = await System.Text.Json.JsonSerializer.DeserializeAsync
                    <IEnumerable<ProductDto>>(contentStream, options);

            return productDtos; 
        }

        private async Task<IEnumerable<StockLevelDto>> FetchStocks()
        {
            var httpRequestMessage = new HttpRequestMessage(
               HttpMethod.Get,
               $"http://localhost:5000/api/stocks")
            {
                Headers = { { HeaderNames.Accept, "application/json" }, }
            };

            var httpClient = httpClientFactory.CreateClient();

            using var httpResponseMessage =
                await httpClient.SendAsync(httpRequestMessage);

            var stockLevelDtos = Enumerable.Empty<StockLevelDto>();

            if (!httpResponseMessage.IsSuccessStatusCode)
                return stockLevelDtos;

            using var contentStream =
                await httpResponseMessage.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            stockLevelDtos = await System.Text.Json.JsonSerializer.DeserializeAsync
                    <IEnumerable<StockLevelDto>>(contentStream, options);

            return stockLevelDtos;
        }


        // POST /api/products
        [HttpPost("products")]
        public async Task<IActionResult> AddProduct(AddProductDto addProductDto)
        {
            var productJson = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(addProductDto),
                Encoding.UTF8,
                Application.Json);

            var httpClient = httpClientFactory.CreateClient();

            using var httpResponseMessage =
                await httpClient.PostAsync("http://localhost:7000/api/products", productJson);

            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            return Created("", responseContent); // 201 Created
        }


        // PUT /api/baskets/{identifier}
        [HttpPut("baskets/{identifier}")]
        public async Task<IActionResult> UpdateBasket(BasketDto basketDto)
        {
            var basketJson = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(basketDto),
                Encoding.UTF8,
                Application.Json);

            var httpClient = httpClientFactory.CreateClient();

            using var httpResponseMessage =
                await httpClient.PutAsync("http://localhost:8000/api/baskets/" + basketDto.Identifier, basketJson);

            return NoContent(); // 204 No Content
        }

        // GET /api/baskets/{identifier}
        [HttpGet("baskets/{identifier}")]
        public async Task<IActionResult> GetBasket(string identifier)
        {
            var httpRequestMessage = new HttpRequestMessage(
               HttpMethod.Get,
               $"http://localhost:8000/api/baskets/{identifier}")
            {
                Headers = { { HeaderNames.Accept, "application/json" }, }
            };

            var httpClient = httpClientFactory.CreateClient();

            using var httpResponseMessage =
                await httpClient.SendAsync(httpRequestMessage);

            BasketDto basketDto = null;

            if (!httpResponseMessage.IsSuccessStatusCode)
                return NotFound();

            using var contentStream =
                await httpResponseMessage.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            basketDto = await System.Text.Json.JsonSerializer.DeserializeAsync<BasketDto>(contentStream, options);

            return Ok(basketDto); // 200 OK
        }


        // POST /api/orders
        [HttpPost("orders")]
        public async Task<IActionResult> AddOrder(OrderDto orderDto)
        {
            var orderJson = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(orderDto),
                Encoding.UTF8,
                Application.Json);

            var httpClient = httpClientFactory.CreateClient();

            using var httpResponseMessage =
                await httpClient.PostAsync("http://localhost:6000/api/orders", orderJson);

            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            var orderIdDto = JsonConvert.DeserializeObject<OrderIdDto>(responseContent);

            return Created("", orderIdDto); // 201 Created
        }
    }
}
