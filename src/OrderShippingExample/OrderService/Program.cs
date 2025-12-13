using MassTransit;
using SharedMessages.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");

        cfg.Message<OrderPlaced>(x => x.SetEntityName("order-topic-exchange"));
        cfg.Publish<OrderPlaced>(x =>
        {
            x.ExchangeType = "topic";
        });

    });
});

// Add services to the container.
var app = builder.Build();

app.MapPost("/orders", async (OrderRequest order, IBus bus) =>
{
    var orderPlacedMessage = new OrderPlaced(order.orderId, order.quantity);

    string routingKey = order.quantity > 10 ? "order.shipping" : "order.regular.tracking";

    await bus.Publish(orderPlacedMessage, context =>
    {
        context.SetRoutingKey(routingKey);

    });

    return Results.Created($"/orders/{order.orderId}", orderPlacedMessage);
});

// Configure the HTTP request pipeline.

app.Run();


public record OrderRequest(Guid orderId, int quantity);


