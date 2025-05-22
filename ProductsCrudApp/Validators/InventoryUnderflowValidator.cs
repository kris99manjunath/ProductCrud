using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProductsCrudApp.ExtensionMethod;
using ProductsCrudApp.Models.ResponseRequest;

namespace ProductsCrudApp.Validators
{
    public class InventoryUnderflowValidator : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            if (context.TryGetArgument("quantity", out int quantity))
            {
                var product = context.HttpContext.GetEntity();
                if (product.StockAvailable < quantity)
                {
                    var response = new ErrorResponse(
                        ErrorCode.INVALID_OPERATION,
                        $"Cannot decrement by {quantity} as only {product.StockAvailable} quantity available.");
                    context.Result = new BadRequestObjectResult
                    (response);
                    return;
                }
            }

            await next();
        }
    }
}
