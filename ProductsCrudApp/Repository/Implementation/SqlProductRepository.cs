using Microsoft.EntityFrameworkCore;
using ProductsCrudApp.ExtensionMethod;
using ProductsCrudApp.Models.ResponseRequest;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCrudApp.Repository
{
    public class SqlProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public SqlProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<Product> AddProductAsync(ProductRequest productDto)
        {
            var product = productDto.ToProduct();
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product, ProductRequest productDto)
        {
            product.UpdateProduct(productDto);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> DecrementStockAsync(Product product, int quantity)
        {
            product.StockAvailable -= quantity;
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> AddToStockAsync(Product product, int quantity)
        {
            product.StockAvailable += quantity;
            await _context.SaveChangesAsync();
            return product;
        }
    }
}

