using FreakyFashionServices.OrderProcessor.Data;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using FreakyFashionServices.OrderProcessor.Models.Domain;
using FreakyFashionServices.OrderProcessor.Models.DTO;
using FreakyFashionServices.OrderProcessor.Services;
using static System.Console;

var context = new OrderServiceContext();

var factory = new ConnectionFactory
{
    Uri = new Uri("amqp://guest:guest@localhost:5672")
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Säkerställ att kö finns - om inte, skapa den.
channel.QueueDeclare(
   // Namnet på kön
   queue: "orders",
   // Vi vill att kön ska finnas kvar även om RabbitMQ startas om
   durable: true,
   // Kön får användas av flera connections.
   exclusive: false,
   // Kön ska inte automatiskt raderas så fort ingen längre finns några subscribers.
   autoDelete: false,
   // Använs av plugins och broker-specifika features, såsom message TTL
   // (time to live), queue length limit, etc.
   arguments: null);

var consumer = new EventingBasicConsumer(channel);

var OrderManager = new OrderManager(context);

consumer.Received += (sender, e) => {

    WriteLine("Processing orders...");

    var body = e.Body.ToArray();
    var json = Encoding.UTF8.GetString(body);

    var dto = JsonConvert.DeserializeObject<OrderDto>(json);

    var order = new Order(
       dto.Identifier,
       dto.Customer
    );

    OrderManager.Register(order);
};

channel.BasicConsume(
   queue: "orders",
   autoAck: true,
   consumer: consumer);

WriteLine(" Press [ENTER] to exit.");
ReadLine();