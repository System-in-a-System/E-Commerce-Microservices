namespace FreakyFashionServices.APIGateway.Models.DTO
{
    public class OrderDto
    {
        public string Identifier { get; set; }
        public string Customer { get; set; }
        public ICollection<OrderLineDto> OrderLineDtos { get; set; }
            = new List<OrderLineDto>();
    }
}
