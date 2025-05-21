using ProductsCrudApp.DTO;

namespace ProductsCrudApp.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(ProductInputDto productDto);
        Task<Product> UpdateProductAsync(Product product, ProductInputDto productDto);
        Task DeleteProductAsync(Product product);
        Task<Product> DecrementStockAsync(Product product, int quantity);
        Task<Product> AddToStockAsync(Product product, int quantity);
    }
}
