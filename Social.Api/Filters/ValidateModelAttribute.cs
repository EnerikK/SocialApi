using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Social.Api.Contracts.Common;
using Social.Application_UseCases_.Enums;

namespace Social.Api.Filters;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        var apiError = new ErrorResponse();

        if (!context.ModelState.IsValid)
        {
            apiError.StatusCode = 1;
            apiError.StatusMessage = "Bad Request";
            apiError.TimeStamp = DateTime.Now;
            var errors = context.ModelState.AsEnumerable();
            foreach (var error in errors) // getting all the errors in the api error list 
            {
                foreach (var modelError in error.Value.Errors)
                {
                    apiError.Errors.Add(modelError.ErrorMessage);
                }
            }

            context.Result = new BadRequestObjectResult(apiError);
            //TODO:Aspnet is overiding the action result, i have to somehow disable that 
        }
    }
}