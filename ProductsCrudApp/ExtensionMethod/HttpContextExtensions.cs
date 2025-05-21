namespace ProductsCrudApp.ExtensionMethod
{
    public static class HttpContextExtensions
    {
        public static Product GetEntity(this HttpContext context)
        {
            var key = $"{typeof(Product).Name}_Entity";
            var product = context.Items[key] as Product;
            if (product is null)
            {
                throw new NullReferenceException("Product is not set");
            }
            else
            {
                return product;
            }
        }

        public static void SetEntity(this HttpContext context, Product product)
        {
            var key = $"{typeof(Product).Name}_Entity";
            context.Items[key] = product;
        }
    }
}
