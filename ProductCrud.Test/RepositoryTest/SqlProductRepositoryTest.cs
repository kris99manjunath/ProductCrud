using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using ProductsCrudApp.Repository;
using ProductsCrudApp.DTO;

namespace ProductsCrudApp.Tests.Repositories
{
    [TestFixture]
    public class SqlProductRepositoryTests
    {
        private ProductDbContext _context;
        private SqlProductRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(databaseName: "TestProductDb")
                .Options;

            _context = new ProductDbContext(options);

            _context.Products.Add(new Product { ProductId = 1, Name = "Test Product", StockAvailable = 10 });
            _context.SaveChanges();

            _repository = new SqlProductRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAllProductsAsync()
        {
            var products = await _repository.GetAllProductsAsync();

            Assert.That(products.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetProductByIdAsync()
        {
            var product = await _repository.GetProductByIdAsync(1);

            Assert.That(product, Is.Not.Null);
            Assert.That(product.ProductId, Is.EqualTo(1));
        }

        [Test]
        public async Task AddProductAsync()
        {
            var dto = new ProductInputDto { Name = "New Product", StockAvailable = 5 };

            var result = await _repository.AddProductAsync(dto);

            Assert.That(result.ProductId, Is.Not.EqualTo(0));
            Assert.That(result.Name, Is.EqualTo("New Product"));
        }

        [Test]
        public async Task UpdateProductAsync()
        {
            var product = await _repository.GetProductByIdAsync(1);
            var dto = new ProductInputDto { Name = "Updated", StockAvailable = 20 };

            var updated = await _repository.UpdateProductAsync(product!, dto);

            Assert.That(updated.Name, Is.EqualTo("Updated"));
            Assert.That(updated.StockAvailable, Is.EqualTo(20));
        }

        [Test]
        public async Task DeleteProductAsync()
        {
            var product = await _repository.GetProductByIdAsync(1);

            await _repository.DeleteProductAsync(product!);

            var result = await _repository.GetProductByIdAsync(1);
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task DecrementStockAsync()
        {
            var product = await _repository.GetProductByIdAsync(1);

            var updated = await _repository.DecrementStockAsync(product!, 3);

            Assert.That(updated.StockAvailable, Is.EqualTo(7));
        }

        [Test]
        public async Task AddToStockAsync()
        {
            var product = await _repository.GetProductByIdAsync(1);

            var updated = await _repository.AddToStockAsync(product!, 5);

            Assert.That(updated.StockAvailable, Is.EqualTo(15));
        }
    }
}
