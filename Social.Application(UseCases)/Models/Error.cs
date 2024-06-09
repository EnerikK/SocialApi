using Social.Application_UseCases_.Enums;

namespace Social.Application_UseCases_.Models;

public class Error
{
    public ErrorCode Code { get; set; }
    public string Message { get; set; }
}