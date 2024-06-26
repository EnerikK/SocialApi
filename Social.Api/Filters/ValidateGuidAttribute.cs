using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Social.Api.Contracts.Common;

namespace Social.Api.Filters;

public class ValidateGuidAttribute : ActionFilterAttribute
{
    private readonly List<string> _keys;

    public ValidateGuidAttribute(string key)
    {
        _keys = new List<string>();
        _keys.Add(key);
    }

    public ValidateGuidAttribute(string key1, string key2)
    {
        _keys = new List<string>();
        _keys.Add(key1);
        _keys.Add(key2);
    }
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        bool isError = false;
        var apiError = new ErrorResponse();
        _keys.ForEach(key =>
        {
            if(!context.ActionArguments.TryGetValue(key,out var value)) return;
            if (!Guid.TryParse(value?.ToString(), out var guid))
            {
                isError = true;
                apiError.Errors.Add($"The id for {key} is not correct format");
            }
        });

        if (isError)
        {
            apiError.StatusCode = 400;
            apiError.StatusMessage = "BadRequest";
            apiError.TimeStamp = DateTime.Now;
            context.Result = new ObjectResult(apiError);
        }
    }
}