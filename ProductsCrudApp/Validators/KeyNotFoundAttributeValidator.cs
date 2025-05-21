using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ProductsCrudApp.Repository;
using ProductsCrudApp.ExtensionMethod;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ProductsCrudApp.ResponseRequest;

namespace ProductsCrudApp.Validators
{
    public class KeyNotFoundAttributeValidator : ActionFilterAttribute
    {
        private readonly IProductRepository _product;

        public KeyNotFoundAttributeValidator(IProductRepository repository)
        {
            _product = repository;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.TryGetArgument("id", out int id))
            {
                var product = await _product.GetProductByIdAsync(id);
                if (product == null)
                {
                    var response = new ErrorResponseRequest(ErrorCode.NOT_FOUND, $"Product with ID {id} not found.");
                    context.Result = new NotFoundObjectResult(response);
                    return;
                }
                context.HttpContext.SetEntity(product);
            }
           
            await next();
        }
    }

}
