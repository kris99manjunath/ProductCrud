using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductsCrudApp.Models.ResponseRequest
{
    public class ProductRequest
    {
        [Required]
        public required string Name { get; set; }

        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock available cannot be negative")]
        public int StockAvailable { get; set; }

        public Product ToProduct()
        {
            return new()
            {
                Name = Name,
                Description = Description,
                Price = Price,
                StockAvailable = StockAvailable
            };
        }

        public Product ToProduct(int id)
        {
            return new()
            {
                ProductId = id,
                Name = Name,
                Description = Description,
                Price = Price,
                StockAvailable = StockAvailable
            };
        }
    }
}
