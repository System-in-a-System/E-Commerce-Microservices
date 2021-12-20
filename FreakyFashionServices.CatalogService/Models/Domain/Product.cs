using System.ComponentModel.DataAnnotations;

namespace FreakyFashionServices.CatalogService.Models.Domain
{
    public class Product
    {
        public Product(int id, 
                       string name, 
                       string description, 
                       string imageUrl, 
                       double price, 
                       string articleNumber)
        {
            Id = id;
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            ArticleNumber = articleNumber;  
        }

        public Product(string name,
                       string description,
                       string imageUrl,
                       double price,
                       string articleNumber)
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            ArticleNumber = articleNumber;
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public string ArticleNumber { get; set; }
        public string UrlSlug => Name.Trim().Replace("-", "").Replace(" ", "-").ToLower();
    }
}
