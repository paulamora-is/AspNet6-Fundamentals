using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNet_Core6.Fundamentals.Attibutes
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Query.TryGetValue(Configuration.ApiKeyName, out var apiKey))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 401,
                    Content = "ApiKey not found"
                };

                return;
            }

            if (!Configuration.ApiKey.Equals(apiKey))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 403,
                    Content = "Unauthorized access"
                };

                return;
            }

            await next();
        }
    }
}
