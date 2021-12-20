namespace FreakyFashionServices.StockService.Models.DTO
{
    public class UpdateStockLevelDto
    {
        public string ArticleNumber { get; set; }
        public int StockLevel { get; set; }
    }
}