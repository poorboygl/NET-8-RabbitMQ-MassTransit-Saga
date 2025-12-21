using MassTransit;
using SharedMessages.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");

        cfg.Message<OrderPlaced>(x => x.SetEntityName("order-placed-exchange"));
        cfg.Publish<OrderPlaced>(x =>
        {
            x.ExchangeType = "direct";
        });

    });
});

// Add services to the container.
var app = builder.Build();

app.MapPost("/orders", async (OrderRequest order, IBus bus) =>
{
    var orderPlacedMessage = new OrderPlaced(order.orderId, order.quantity);

    var headers = new Dictionary<string, object>();

    await bus.Publish(orderPlacedMessage, context => {

        context.SetRoutingKey("order.created");
    });

    return Results.Created($"/orders/{order.orderId}", orderPlacedMessage);
});

// Configure the HTTP request pipeline.

app.Run();


public record OrderRequest(Guid orderId, int quantity);


