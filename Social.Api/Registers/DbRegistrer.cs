
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Social.DataAccess;

namespace Social.Api.Registers
{
    public class DbRegistrer : IWebApplicationBuilderRegistar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            var ConnectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<DataContext>(Options =>
            {
                Options.UseSqlServer(ConnectionString);
            });

            builder.Services.AddIdentityCore<IdentityUser>(Options =>
                {
                    Options.Password.RequireDigit = false;
                    Options.Password.RequiredLength = 5;
                    Options.Password.RequireLowercase = false;
                    Options.Password.RequireUppercase = false;
                    Options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<DataContext>();
        }
    }
}
