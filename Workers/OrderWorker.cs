using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using System.Text.Json;
using OrderApi.Enums;
using OrderApi.Services.Interfaces;
using OrderApi.Models;

public class OrderWorker : BackgroundService
{
    private readonly AzureServiceBusOptions _serviceBusOptions;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OrderWorker> _logger;
    private ServiceBusClient? _client;
    private ServiceBusProcessor? _processor;

    public OrderWorker(IOptions<AzureServiceBusOptions> serviceBusOptions, IServiceProvider serviceProvider, ILogger<OrderWorker> logger) : base()
    {
        _serviceBusOptions = serviceBusOptions.Value;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }


    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _client = new ServiceBusClient(_serviceBusOptions.ConnectionString);
        _processor = _client.CreateProcessor(_serviceBusOptions.QueueName, new ServiceBusProcessorOptions());

        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;

        await _processor.StartProcessingAsync(cancellationToken);

        _logger.LogInformation("OrderWorker started.");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.CompletedTask; // O processamento é iniciado no StartAsync
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

            string body = args.Message.Body.ToString();
            var order = JsonSerializer.Deserialize<Order>(body);

            if (order == null)
            {
                _logger.LogWarning("Order deserialization returned null.");
                return;
            }

            _logger.LogInformation($"Received order: {order.Id}");

            // Simular processamento
            await Task.Delay(5000);

            await orderService.UpdateOrderStatusAsync(order.Id, OrderStatus.Finalizado);

            await args.CompleteMessageAsync(args.Message);
        }
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Error processing message.");
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_processor != null)
        {
            await _processor.StopProcessingAsync(cancellationToken);
            await _processor.DisposeAsync();
        }

        if (_client != null)
        {
            await _client.DisposeAsync();
        }

        _logger.LogInformation("OrderWorker stopped.");
    }
}