using MassTransit;
using static SharedMessages.Messages;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderPlacedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");
        cfg.ReceiveEndpoint("order-placed", e =>
        {
            e.Consumer<OrderPlacedConsumer>();
        });
    });
});

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.Run();

public class OrderPlacedConsumer : IConsumer<OrderPlaced>
{
    public async Task Consume(ConsumeContext<OrderPlaced> context)
    {
        Console.WriteLine($"Inventory reserved for Order {context.Message.OrderId}");
        await context.Publish(new InventoryReserved(context.Message.OrderId));
    }
}