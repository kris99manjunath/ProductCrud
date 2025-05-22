using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProductsCrudApp.ExtensionMethod;
using ProductsCrudApp.Models.ResponseRequest;

namespace ProductsCrudApp.Validators
{
    public class InventoryOverflowValidator : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            if (context.TryGetArgument("quantity", out int quantity))
            {
                var product = context.HttpContext.GetEntity();
                if ((long)product.StockAvailable + quantity > Int32.MaxValue)
                {
                    var response = new ErrorResponse(
                        ErrorCode.INVALID_OPERATION,
                        $"Cannot add by {quantity} as inventory is " +
                        $"{product.StockAvailable} available and maxInventorySize is {Int32.MaxValue}.");
                    context.Result = new BadRequestObjectResult
                    (response);
                    return;
                }
            }

            await next();
        }
    }
}
