using FreakyFashionServices.StockService.Data;
using FreakyFashionServices.StockService.Models.Domain;
using FreakyFashionServices.StockService.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FreakyFashionServices.StockService.Controllers
{
    // TODO: Kalla denna för StockLevelController?
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        public StockController(StockServiceContext context)
        {
            Context = context;
        }

        private StockServiceContext Context { get; }

        [HttpPut("{articleNumber}")]
        public IActionResult UpdateStockLevel(string articleNumber, UpdateStockLevelDto updateStockLevelDto)
        {
            var stockLevel = Context.StockLevel
                .FirstOrDefault(x => x.ArticleNumber == updateStockLevelDto.ArticleNumber);

            if (stockLevel == null)
            {
                // Alternativt, använd AutoMapper
                stockLevel = new StockLevel(
                    updateStockLevelDto.ArticleNumber,
                    updateStockLevelDto.StockLevel
                );

                Context.StockLevel.Add(stockLevel);
            }
            else
            {
                stockLevel.Stock = updateStockLevelDto.StockLevel;
            }

            Context.SaveChanges();

            return NoContent(); // 204 No Content
        }

        [HttpGet]
        public IEnumerable<StockLevelDto> GetAll()
        {
            var stockLevelDtos = Context.StockLevel.Select(x => new StockLevelDto
            {
                ArticleNumber = x.ArticleNumber,
                Stock = x.Stock
            });

            return stockLevelDtos;
        }
    }

    public class StockLevelDto
    {
        public string ArticleNumber { get; set; }
        public int Stock { get; set; }
    }

}
