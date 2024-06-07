using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Social.Api.Registers
{
    public class MVCWebAppRegistrar : IWebApplicationRegistar
    {
        public void RegisterPipelineComponents(WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>(); //resolves the service

                foreach (var description in provider.ApiVersionDescriptions)
                {// for each var creates a swagger End Point
                    options.SwaggerEndpoint($"/Swagger/{description.GroupName}/Swagger.json",
                    description.ApiVersion.ToString());//string interpolation 
                }

            });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
