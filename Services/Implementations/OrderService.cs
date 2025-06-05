using OrderApi.Models;
using OrderApi.Repositories.Interfaces;
using OrderApi.Services.Interfaces;
using Azure.Messaging.ServiceBus;
using OrderApi.Enums;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace OrderApi.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly AzureServiceBusOptions _serviceBusOptions;
        private readonly IOrderRepository _orderRepository;
        private readonly IWebSocketHandler _webSocketHandler;

        public OrderService(IOrderRepository orderRepository, IOptions<AzureServiceBusOptions> serviceBusOptions, IWebSocketHandler webSocketHandler)
        {
            _orderRepository = orderRepository;
            _serviceBusOptions = serviceBusOptions.Value;
            _webSocketHandler = webSocketHandler;

        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }
        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }
        public async Task CreateOrderAsync(Order order)
        {
            await _orderRepository.AddAsync(order);
            await SendMessageToServiceBusAsync(order);
        }

        public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                await _orderRepository.UpdateAsync(order);

                var message = JsonSerializer.Serialize(order);
                await _webSocketHandler.SendMessageAsync(message);
            }
        }

        public async Task UpdateOrderAsync(Order order, OrderDto orderDto)
        {
            order.Cliente = orderDto.Cliente;
            order.Produto = orderDto.Produto;
            order.Valor = orderDto.Valor;

            await _orderRepository.UpdateAsync(order);
            await SendMessageToServiceBusAsync(order);
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            await _orderRepository.DeleteAsync(id);
        }

        private async Task SendMessageToServiceBusAsync(Order order)
        {
            await using var client = new ServiceBusClient(_serviceBusOptions.ConnectionString);
            ServiceBusSender sender = client.CreateSender(_serviceBusOptions.QueueName);

            try
            {
                await UpdateOrderStatusAsync(order.Id, OrderStatus.Processando);
                string messageBody = JsonSerializer.Serialize(order);
                ServiceBusMessage message = new ServiceBusMessage(messageBody);

                await sender.SendMessageAsync(message);
                Console.WriteLine($"Mensagem enviada: {messageBody}");
            }
            finally
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}