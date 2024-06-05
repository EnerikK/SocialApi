namespace Social.Api.Registers
{
    public interface IWebApplicationRegistar : IRegistrar
    {
        public void RegisterPipelineComponents(WebApplication app);
    }
}
