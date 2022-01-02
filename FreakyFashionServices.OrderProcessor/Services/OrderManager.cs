using FreakyFashionServices.OrderProcessor.Data;
using FreakyFashionServices.OrderProcessor.Models.Domain;

namespace FreakyFashionServices.OrderProcessor.Services
{
    public class OrderManager
    {
        public OrderManager(OrderServiceContext context)
        {
            Context = context;
        }

        private OrderServiceContext Context { get; }

        public void Register(Order order)
        {
            Thread.Sleep(2000);

            Context.Orders.Add(order);

            Context.SaveChanges();
        }
    }
}
