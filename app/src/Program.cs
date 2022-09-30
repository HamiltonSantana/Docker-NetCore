using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ServerSide;
using ServerSide.Authentication;
using ServerSide.Authentication.Interface;
using ServerSide.Interface;
using ServerSide.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>();
var connfiguration = builder.Configuration;
builder.Services.Configure<AuthSettings>(connfiguration.GetSection(AuthSettings.SectionName));

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(bearer =>
{
    var authSettings = connfiguration.GetSection(AuthSettings.SectionName).GetValue<string>("Secret");
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
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();

using (var context = new ApplicationDbContext())
{
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<SignalHub>("/notification");

app.Run();
