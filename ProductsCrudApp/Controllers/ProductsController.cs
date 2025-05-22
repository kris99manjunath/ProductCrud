using Microsoft.AspNetCore.Mvc;
using ProductsCrudApp.ExtensionMethod;
using ProductsCrudApp.Models.ResponseRequest;
using ProductsCrudApp.Repository;
using ProductsCrudApp.Validators;
using System.ComponentModel.DataAnnotations;

namespace ProductsCrudApp.Controllers;

[ApiController]
[Route("[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _products;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductRepository _products, ILogger<ProductsController> logger)
    {
        this._products = _products;
        _logger = logger;
    }

    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    [HttpPost]
    public async Task<IActionResult> Create(ProductRequest product)
    {
        var savedProduct = await _products.AddProductAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = savedProduct.ProductId }, savedProduct);
    }

    [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _products.GetAllProductsAsync();
        return Ok(products);
    }

    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ServiceFilter(typeof(KeyNotFoundValidator))]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = HttpContext.GetEntity();
        return Ok(product);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{id}")]
    [ServiceFilter(typeof(KeyNotFoundValidator))]
    public async Task<IActionResult> Delete(int id)
    {
        var product = HttpContext.GetEntity();
        await _products.DeleteProductAsync(product);
        return NoContent();
    }

    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [HttpPut("{id}")]
    [ServiceFilter(typeof(KeyNotFoundValidator))]
    public async Task<IActionResult> Update(int id, ProductRequest updatedProductDto)
    {
        var product = HttpContext.GetEntity();
        return Ok(await _products.UpdateProductAsync(product, updatedProductDto));
    }

    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [HttpPut("decrement-stock/{id}/{quantity}")]
    [ServiceFilter(typeof(KeyNotFoundValidator))]
    [ServiceFilter(typeof(InventoryUnderflowValidator))]
    public async Task<IActionResult> DecrementStock(int id, [PositiveNumberAttributeValidator] int quantity)
    {
        var product = HttpContext.GetEntity();
        return Ok(await _products.DecrementStockAsync(product, quantity));
    }

    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [HttpPut("add-to-stock/{id}/{quantity}")]
    [ServiceFilter(typeof(KeyNotFoundValidator))]
    [ServiceFilter(typeof(InventoryOverflowValidator))]
    public async Task<IActionResult> AddToStock(int id, [PositiveNumberAttributeValidator] int quantity)
    {
        var product = HttpContext.GetEntity();
        return Ok(await _products.AddToStockAsync(product, quantity));
    }
}
