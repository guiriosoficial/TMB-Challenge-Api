using System.ComponentModel.DataAnnotations;

namespace OrderApi.Models
{
    public class OrderDto
    {
        [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do cliente deve ter no máximo 100 caracteres.")]
        public required string Cliente { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do cliente deve ter no máximo 100 caracteres.")]
        public required string Produto { get; set; }

        [Required(ErrorMessage = "O valor do produto é obrigatório.")]
        [Range(0.0, (double)decimal.MaxValue, ErrorMessage = "O valor do produto deve ser um número não negativo.")]
        public required decimal Valor { get; set; }
    }
}