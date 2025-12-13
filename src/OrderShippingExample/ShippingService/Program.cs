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

            e.Bind("order-headers-exchange", x =>
            {

                x.ExchangeType = "headers";
                x.SetBindingArgument("department", "shipping");
                x.SetBindingArgument("priority", "high");
                x.SetBindingArgument("x-match", "all");
            });

        });
    });
});


// Add services to the container.
var app = builder.Build();
// Configure the HTTP request pipeline.

app.Run();


