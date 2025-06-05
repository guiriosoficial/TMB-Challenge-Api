using OrderApi.Enums;

namespace OrderApi.Models
{
    public class OrderDto
    {
        public required string Cliente { get; set; }
        public required string Produto { get; set; }
        public required decimal Valor { get; set; }
    }
}