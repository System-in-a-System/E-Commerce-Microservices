namespace FreakyFashionServices.BasketService.Models.DTO
{
    public class BasketDto
    {
        public string Identifier { get; set; }
        public List<OrderLineDto> Items { get; set; } 
            = new List<OrderLineDto>();
    }
}
