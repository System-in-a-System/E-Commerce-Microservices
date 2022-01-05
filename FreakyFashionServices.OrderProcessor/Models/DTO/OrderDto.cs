namespace FreakyFashionServices.OrderProcessor.Models.DTO
{
    public class OrderDto
    {
        public string Identifier { get; set; }
        public string Customer { get; set; }
        public string OrderKey { get; set; }
        public ICollection<OrderLineDto> OrderLines { get; set; }
            = new List<OrderLineDto>();
    }
}
