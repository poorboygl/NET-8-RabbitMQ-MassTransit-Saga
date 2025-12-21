using MassTransit;
using ShippingService.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    //register Consumer
    x.AddConsumer<OrderPlacedConsumer>();

    x.UsingRabbitMq((context, cfg) => {
        cfg.Host("rabbitmq://localhost");

        cfg.ReceiveEndpoint("shipping-order-queue", e =>
        {

            e.ConfigureConsumer<OrderPlacedConsumer>(context);

            e.Bind("order-placed-exchange", x =>
            {
                x.ExchangeType = "direct";
                x.RoutingKey = "order.created";
            });

            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
        });
    });
});


// Add services to the container.
var app = builder.Build();
// Configure the HTTP request pipeline.

app.Run();


