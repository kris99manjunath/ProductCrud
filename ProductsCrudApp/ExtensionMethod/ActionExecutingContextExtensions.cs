using Microsoft.AspNetCore.Mvc.Filters;

namespace ProductsCrudApp.ExtensionMethod
{
    public static class ActionExecutingContextExtensions
    {
        public static bool TryGetArgument<T>(this ActionExecutingContext context, string key, out T result)
        {
            result = default!;
            if (context.ActionArguments.TryGetValue(key, out var value) && value is T typedValue)
            {
                result = typedValue;
                return true;
            }

            return false;
        }
    }

}
