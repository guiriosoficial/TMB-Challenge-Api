using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Repositories.Implementations;
using OrderApi.Repositories.Interfaces;
using OrderApi.Services.Implementations;
using OrderApi.Services.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AzureServiceBusOptions>(builder.Configuration.GetSection("AzureServiceBus"));
builder.Services.AddTransient<OrderService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Adicionar o DbContext ao contêiner de serviços
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IWebSocketHandler, WebSocketHandler>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddHostedService<OrderWorker>();

// Trata serealização camel case e pascal case e de enums
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

app.UseCors("AllowAll");
app.MapControllers();
app.UseWebSockets();
app.Run();
