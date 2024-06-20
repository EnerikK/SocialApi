using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Social.Application_UseCases_.Options;
using Microsoft.AspNetCore.Identity;

namespace Social.Api.Registers;

public class IdentityRegistrar : IWebApplicationBuilderRegistar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        var jwtSettings = new JWTSettings();
        builder.Configuration.Bind(nameof(jwtSettings),jwtSettings);

        var jwtSection = builder.Configuration.GetSection(nameof(jwtSettings));
        builder.Services.Configure<JWTSettings>(jwtSection);

        builder.Services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                //Validation Settings
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    //Signing Key validation when it gets created
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SigningKey)),
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudiences = jwtSettings.Audience,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
                jwt.Audience = jwtSettings.Audience[0];
                jwt.ClaimsIssuer = jwtSettings.Issuer;
            });

    }
}