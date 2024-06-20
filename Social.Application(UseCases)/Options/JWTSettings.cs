namespace Social.Application_UseCases_.Options;

public class JWTSettings
{
    public string SigningKey { get; set; }
    public string Issuer { get; set; }
    public string[] Audience { get; set; }
}