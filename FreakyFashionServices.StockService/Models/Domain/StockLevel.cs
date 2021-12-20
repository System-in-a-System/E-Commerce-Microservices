using System.ComponentModel.DataAnnotations;

namespace FreakyFashionServices.StockService.Models.Domain
{
    public class StockLevel
    {
        public StockLevel(string articleNumber, int stock)
        {
            ArticleNumber = articleNumber;
            Stock = stock;
        }

        [Key]
        public string ArticleNumber { get; set; }
        public int Stock { get; set; }
    }
}
