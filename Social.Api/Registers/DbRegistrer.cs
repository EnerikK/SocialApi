
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Social.DataAccess;

namespace Social.Api.Registers
{
    public class DbRegistrer : IWebApplicationBuilderRegistar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            var ConnectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<DataContext>(Options => Options.UseSqlServer(ConnectionString));
        }
    }
}
