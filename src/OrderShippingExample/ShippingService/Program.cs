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

            //e.UseMessageRetry(r => r.Exponential(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(5)));

            e.UseKillSwitch(opt => opt.SetActivationThreshold(10)
                                      .SetTripThreshold(0.15)
                                      .SetRestartTimeout(m: 1)
            );
        });
    });
});


// Add services to the container.
var app = builder.Build();
// Configure the HTTP request pipeline.

app.Run();


