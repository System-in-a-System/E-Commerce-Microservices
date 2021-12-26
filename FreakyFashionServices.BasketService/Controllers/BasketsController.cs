using FreakyFashionServices.BasketService.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace FreakyFashionServices.BasketService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        public BasketsController(IDistributedCache cache)
        {
            Cache = cache;
        }

        public IDistributedCache Cache { get; }
        
        // PUT /api/baskets/{identifier}
        [HttpPut("{identifier}")]
        public IActionResult CreateBasket(string identifier, BasketDto basketDto)
        {
            var serializedBasket = JsonSerializer.Serialize(basketDto);

            Cache.SetString(identifier, serializedBasket);
            
            return NoContent(); // 204 No Content
        }

        // GET /api/baskets/{identifier}
        [HttpGet("{identifier}")]
        public ActionResult<BasketDto> GetBasket(string identifier)
        {
            var serializedBasket = Cache.GetString(identifier);

            if(serializedBasket == null)
                return NotFound(); // 404 Not Found

            var basketDto = JsonSerializer.Deserialize<BasketDto>(serializedBasket);

            return Ok(basketDto);
        }

    }
}
