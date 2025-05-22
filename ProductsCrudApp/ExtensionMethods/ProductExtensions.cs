using ProductsCrudApp.Models.ResponseRequest;

namespace ProductsCrudApp.ExtensionMethod
{
    public static class ProductExtensions
    {
        public static void UpdateProduct(this Product product, ProductRequest productDto)
        {
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.StockAvailable = productDto.StockAvailable;
        }
    }
}
