namespace FreakyFashionServices.APIGateway.Models.DTO
{
    public class AddProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public string ArticleNumber { get; set; }
    }
}
