using MassTransit;
using TrackingService.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMassTransit(x =>
{
    //register Consumer
    x.AddConsumer<OrderPlacedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");

        cfg.ReceiveEndpoint("tracking-order-queue", e =>
        {
            e.ConfigureConsumer<OrderPlacedConsumer>(context);

            e.Bind("order-placed-exchange", x =>
            {
                x.RoutingKey = "order.tracking";
                x.ExchangeType = "direct";
            });

        });

    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.



app.Run();

