using MassTransit;
using static SharedMessages.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");
    });
});

// Add services to the container.
var app = builder.Build();

app.MapPost("/orders", async (OrderDto order, IBus bus) =>
{
    var orderPlacedMessage = new OrderPlaced(order.OrderId, order.Quantity);
    await bus.Publish(orderPlacedMessage);
    return Results.Created($"/orders/{order.OrderId}", orderPlacedMessage);
});

// Configure the HTTP request pipeline.

app.Run();

public record OrderDto(Guid OrderId, int Quantity);


