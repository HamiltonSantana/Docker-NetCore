using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServerSide.Infra.Authentication;
using ServerSide.Infra.Authentication.Interface;
using ServerSide.Infra.Context;
using ServerSide.Infra.Repository;
using ServerSide.Infra.Repository.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ServerSide.Infra;
public static class InfraDependecy
{
    public static void ServiceInjection(IServiceCollection service, IConfiguration configuration)
    {
        service.AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(bearer =>
        {
            var authSettings = configuration.GetSection(AuthSettings.SectionName).GetValue<string>("Secret");
            bearer.RequireHttpsMetadata = false;
            bearer.SaveToken = true;
            bearer.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.Latin1.GetBytes(authSettings)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        service.AddDbContext<DatabaseContext>();
        service.AddScoped<IAuthenticationService, AuthenticationService>();
        service.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        service.AddScoped<IUserRepository, UserRepository>();
        service.AddSignalRCore();
    }
}
