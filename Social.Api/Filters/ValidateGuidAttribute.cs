using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Social.Api.Contracts.Common;

namespace Social.Api.Filters;

public class ValidateGuidAttribute : ActionFilterAttribute
{
    private readonly string _key;

    public ValidateGuidAttribute(string key)
    {
        _key = key;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.TryGetValue(_key, out var value))
        {
            if (!Guid.TryParse(value.ToString(), out var guid))
            {
                var apiError = new ErrorResponse();
                apiError.StatusCode = 1;
                apiError.StatusMessage = "Bad Request";
                apiError.TimeStamp = DateTime.Now;
                apiError.Errors.Add($"The id for {_key} is not correct Guid format");
                context.Result = new ObjectResult(apiError);
            }
        }
    }
}