namespace ProductApi.Models
{
    public class Product
    {
        public int Id { get; set; } // Identificador único do produto

        public string Name { get; set; } = string.Empty; // Inicializa com uma string vazia
        public string Description { get; set; } = string.Empty; // Inicializa com uma string vazia
        public decimal Price { get; set; } // Preço do produto
    }
}
