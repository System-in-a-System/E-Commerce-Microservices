namespace FreakyFashionServices.APIGateway.Models.DTO
{
    public class ProductAndStockDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public string ArticleNumber { get; set; }
        public string UrlSlug { get; set; }

        public int Stock { get; set; }
    }
}
