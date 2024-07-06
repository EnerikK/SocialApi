using Social.Application_UseCases_.Enums;

namespace Social.Application_UseCases_.Models;

public class OperationResult<T>
{
    public T PayLoad { get; set; }
    public bool IsError { get; private set; }
    public List<Error> Errors { get; } = new List<Error>();
    //Add an error to the error list and set the IsError to true
    public void AddError(ErrorCode code, string message)
    {
        HandleError(code,message);
    }
    //Adds a UnknownError error to the error list
    public void AddUnknownError(string message)
    {
        HandleError(ErrorCode.UnknownError,message);
    }
    //reset the error flag to false
    private void ResetIsError()
    {
        IsError = false;
    }
    private void HandleError(ErrorCode code, string message)
    {
        Errors.Add(new Error()
        {
            Code = code,
            Message = message
        });
        IsError = true;
    }

}