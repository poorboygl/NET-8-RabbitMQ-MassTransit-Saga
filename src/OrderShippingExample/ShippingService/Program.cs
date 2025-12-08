using MassTransit;
using ShippingService.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) => {
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


