using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Social.Api.Contracts.Common;

namespace Social.Api.Filters;

public class SocialExceptionHandler : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var apiError = new ErrorResponse();
        apiError.StatusCode = 2;
        apiError.StatusMessage = "Sever Error";
        apiError.TimeStamp = DateTime.Now;
        apiError.Errors.Add(context.Exception.Message);

        context.Result = new JsonResult(apiError) { StatusCode = 2 };
    }
}