using OrderApi.Enums;

namespace OrderApi.Models
{
    public class Order : OrderDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public OrderStatus Status { get; set; } = OrderStatus.Pendente;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}