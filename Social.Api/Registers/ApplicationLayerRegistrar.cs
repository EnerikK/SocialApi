using Social.Application_UseCases_.Services;

namespace Social.Api.Registers;

public class ApplicationLayerRegistrar : IWebApplicationBuilderRegistar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IdentityService>();
    }
}