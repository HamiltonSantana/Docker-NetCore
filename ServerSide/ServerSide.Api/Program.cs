using ServerSide.Infra;
using ServerSide.Infra.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<IConfiguration>();
InfraDependecy.ServiceInjection(builder.Services, configuration: builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", _ => _.WithOrigins("http://localhost:4200")
    .WithMethods("GET", "POST", "DELETE", "PUT")
    .AllowAnyHeader()
    .AllowCredentials());
});

var app = builder.Build();

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
