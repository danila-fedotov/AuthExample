using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace EmptyPlatform.Web
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var argumentNullException = context.Exception as ArgumentNullException;

            if (argumentNullException is not null)
            {
                context.Exception = null;
                context.ModelState.AddModelError(argumentNullException.ParamName, argumentNullException.Message);
                context.Result = new BadRequestObjectResult(context.ModelState);
            }

            // TODO: log error
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
