using MassTransit;
using SharedMessages.Messages;

namespace TrackingService.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlaced>
    {
        public Task Consume(ConsumeContext<OrderPlaced> context)
        {
            if (context.Message.Quantity <= 0)
            {
                Console.WriteLine($"Rejected order with ID: {context.Message.OrderId}");
                throw new Exception("Invalid quantity, rejecting the message.");
            }

            Console.WriteLine($"Processed order with ID: {context.Message.OrderId}");
            return Task.CompletedTask;
        }
    }
}
