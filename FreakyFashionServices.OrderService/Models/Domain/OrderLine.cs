using System.ComponentModel.DataAnnotations;

namespace FreakyFashionServices.OrderService.Models.Domain
{
    public class OrderLine
    {
        public OrderLine(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public OrderLine(int orderLineId, int productId, int quantity)
        {
            OrderLineId = orderLineId;
            ProductId = productId;
            Quantity = quantity;
        }

        [Key]
        public int OrderLineId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}