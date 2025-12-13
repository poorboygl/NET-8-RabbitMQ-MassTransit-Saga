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

        cfg.ReceiveEndpoint("tracking-order-placed", e =>
        {

            e.ConfigureConsumer<OrderPlacedConsumer>(context);

            e.Bind("order-headers-exchange", x =>
            {

                x.ExchangeType = "headers";
                x.SetBindingArgument("department", "tracking");
                x.SetBindingArgument("priority", "low");
                x.SetBindingArgument("x-match", "all");
            });

        });

    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.



app.Run();

