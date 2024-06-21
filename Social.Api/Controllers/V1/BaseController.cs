using Microsoft.AspNetCore.Mvc;
using Social.Api.Contracts.Common;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.Models;
namespace Social.Api.Controllers.V1;

public class BaseController : ControllerBase
{
    protected IActionResult HandleErrorResponse(List<Error> errors)
    {
        var apiError = new ErrorResponse();

        if (errors.Any(error => error.Code == ErrorCode.NotFound))
        {
            var error = errors.First(error => error.Code == ErrorCode.NotFound);
            
            apiError.StatusCode = 1;
            apiError.StatusMessage = "Not Found";
            apiError.TimeStamp = DateTime.Now;
            apiError.Errors.Add(error.Message);

            return NotFound(apiError);
        }
        
        apiError.StatusCode = 400;
        apiError.StatusMessage = "Bad request";
        apiError.TimeStamp = DateTime.Now;
        errors.ForEach(error => apiError.Errors.Add(error.Message));
        return StatusCode(400, apiError);

    }
}