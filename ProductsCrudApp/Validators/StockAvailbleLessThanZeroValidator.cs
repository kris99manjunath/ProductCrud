using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProductsCrudApp.ExtensionMethod;
using ProductsCrudApp.ResponseRequest;


namespace ProductsCrudApp.Validators
{
    public class StockAvailbleLessThanZeroValidator : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            if (context.TryGetArgument("quantity", out int quantity))
            {
                var product = context.HttpContext.GetEntity();
                if (product.StockAvailable < quantity)
                {
                    var response = new ErrorResponseRequest(
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
