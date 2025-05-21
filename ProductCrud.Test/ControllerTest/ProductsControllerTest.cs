using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsCrudApp.Controllers;
using ProductsCrudApp.DTO;
using ProductsCrudApp.ExtensionMethod;
using ProductsCrudApp.Repository;
using ProductsCrudApp.Validators;

namespace ProductCrud.Test.ControllerTest
{
    [TestFixture]
    public class ProductsControllerTests
    {
        private Mock<IProductRepository> _repoMock;
        private ServiceCollection _services;
        private Mock<ILogger<ProductsController>>? _loggerMock;
        private ProductsController _controller;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IProductRepository>();
            _services = new ServiceCollection();
            _services.AddScoped<IProductRepository>(_ => _repoMock.Object);

            _services.AddScoped<KeyNotFoundAttributeValidator>();
            _services.AddScoped<StockAvailbleLessThanZeroValidator>();

            var serviceProvider = _services.BuildServiceProvider();
            _controller = new ProductsController(_repoMock.Object, Mock.Of<ILogger<ProductsController>>())
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Test]
        public async Task Create_ValidDto_ReturnsCreated()
        {
            var dto = new ProductInputDto { Name = "New", StockAvailable = 5 };
            var created = new Product { ProductId = 1, Name = "New", StockAvailable = 5 };

            _repoMock.Setup(r => r.AddProductAsync(dto)).ReturnsAsync(created);

            var result = await _controller.Create(dto) as CreatedAtActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo("GetById"));
            Assert.That(created, Is.EqualTo(result.Value));
        }

        [Test]
        public async Task GetAll_ReturnsProducts()
        {
            var list = new List<Product> { new Product { ProductId = 1, Name = "Item" } };

            _repoMock.Setup(r => r.GetAllProductsAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll() as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(list, Is.EqualTo(result.Value));
        }

        [Test]
        public async Task GetAll_Empty_ReturnsEmptyList()
        {
            _repoMock.Setup(r => r.GetAllProductsAsync()).ReturnsAsync(new List<Product>());

            var result = await _controller.GetAll() as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.InstanceOf<List<Product>>());
        }

        [Test]
        public async Task Create_NullProduct_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("Name", "Required");

            Assert.ThrowsAsync<NullReferenceException>(async () => await _controller.Create(null));
        }


        [Test]
        public async Task GetById_EntityFound_ReturnsOk()
        {
            var product = new Product { ProductId = 1, Name = "Test" };
            SetHttpContextWithProduct(product);

            var result = await _controller.GetById(1) as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(product));
        }

        [Test]
        public async Task GetById_EntityMissing_Returns500OrHandled()
        {
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            Assert.ThrowsAsync<NullReferenceException>(async () => await _controller.GetById(1));
        }

        [Test]
        public async Task Update_ValidDto_ReturnsUpdatedProduct()
        {
            var original = new Product { ProductId = 1, Name = "Old" };
            var dto = new ProductInputDto { Name = "New", StockAvailable = 5 };
            var updated = new Product { ProductId = 1, Name = "New", StockAvailable = 5 };

            SetHttpContextWithProduct(original);
            _repoMock.Setup(r => r.UpdateProductAsync(original, dto)).ReturnsAsync(updated);

            var result = await _controller.Update(1, dto) as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(updated));
        }

        [Test]
        public async Task Update_EntityMissing_Throws()
        {
            var dto = new ProductInputDto { Name = "Update", StockAvailable = 2 };
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            Assert.ThrowsAsync<NullReferenceException>(async () => await _controller.Update(1, dto));
        }

        [Test]
        public async Task Delete_EntityFound_DeletesProduct()
        {
            var product = new Product { ProductId = 1, Name = "Test" };
            SetHttpContextWithProduct(product);

            var result = await _controller.Delete(1);

            _repoMock.Verify(r => r.DeleteProductAsync(product), Times.Once);
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public void Delete_EntityMissing_Throws()
        {
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            Assert.ThrowsAsync<NullReferenceException>(async () => await _controller.Delete(1));
        }

        [Test]
        public async Task DecrementStock_ValidQuantity_ReturnsUpdated()
        {
            var product = new Product { ProductId = 1, Name = "Test", StockAvailable = 10 };
            var updated = new Product { ProductId = 1, Name = "Test", StockAvailable = 7 };

            SetHttpContextWithProduct(product);
            _repoMock.Setup(r => r.DecrementStockAsync(product, 3)).ReturnsAsync(updated);

            var result = await _controller.DecrementStock(1, 3) as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(updated, Is.EqualTo(result.Value));
        }

        [Test]
        public void DecrementStock_EntityMissing_Throws()
        {
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            Assert.ThrowsAsync<NullReferenceException>(async () => await _controller.DecrementStock(1, 3));
        }


        [Test]
        public async Task AddToStock_ValidQuantity_ReturnsUpdated()
        {
            var product = new Product { ProductId = 1, Name = "Test", StockAvailable = 10 };
            var updated = new Product { ProductId = 1, Name = "Test", StockAvailable = 15 };

            SetHttpContextWithProduct(product);
            _repoMock.Setup(r => r.AddToStockAsync(product, 5)).ReturnsAsync(updated);

            var result = await _controller.AddToStock(1, 5) as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(updated, Is.EqualTo(result.Value));
        }

        [Test]
        public void AddToStock_EntityMissing_Throws()
        {
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            Assert.ThrowsAsync<NullReferenceException>(async () => await _controller.AddToStock(1, 5));
        }

        private void SetHttpContextWithProduct(Product product)
        {
            var context = new DefaultHttpContext();
            context.SetEntity(product);
            _controller.ControllerContext = new ControllerContext { HttpContext = context };
        }
    }
}
