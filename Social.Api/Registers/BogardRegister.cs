
using MediatR;
using Social.Application_UseCases_.UserProfiles.Queries;

namespace Social.Api.Registers
{
    public class BogardRegister : IWebApplicationBuilderRegistar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(Program), typeof(GetAllUserProfiles));
            builder.Services.AddMediatR(typeof(GetAllUserProfiles));
        }
    }
}
