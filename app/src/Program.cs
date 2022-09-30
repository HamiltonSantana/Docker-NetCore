using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServerSide;
using ServerSide.Authentication;
using ServerSide.Authentication.Interface;
using ServerSide.Interface;
using ServerSide.Models;
using ServerSide.Repository;
using ServerSide.Repository.Interface;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.Configure<AuthSettings>(configuration.GetSection(AuthSettings.SectionName));

builder.Services.AddAuthentication(auth =>
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


builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IConfiguration>(configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", _ => _.WithOrigins("http://localhost:4200")
    .WithMethods("GET", "POST", "DELETE", "PUT")
    .AllowAnyHeader()
    .AllowCredentials());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();

var app = builder.Build();

using (var context = new DatabaseContext())
{
    context.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<SignalHub>("/notification");

app.Run();
