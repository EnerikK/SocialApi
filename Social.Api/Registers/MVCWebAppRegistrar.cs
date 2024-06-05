
namespace Social.Api.Registers
{
    public class MVCWebAppRegistrar : IWebApplicationRegistar
    {
        public void RegisterPipelineComponents(WebApplication app)
        {
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
