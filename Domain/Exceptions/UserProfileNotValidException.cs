namespace Social.Domain.Exceptions;

public class UserProfileNotValidException : Exception
{
    internal UserProfileNotValidException()
    {
        ValidationErrors = new List<string>();
    }

    internal UserProfileNotValidException(string message) : base(message)
    {
        ValidationErrors = new List<string>();
    }

    public List<string> ValidationErrors { get; set; }
}