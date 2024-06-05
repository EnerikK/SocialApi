namespace Social.Api.Registers
{
    public interface IWebApplicationBuilderRegistar : IRegistrar
    {
        void RegisterServices(WebApplicationBuilder builder);
    }
}
