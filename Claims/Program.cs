using Claims.Capabilities;
using Claims.Extensions;
using Claims.Infrastructure.Extensions;

var builder = WebApplication
    .CreateBuilder(args)
    .SetupSerilog();

// Add services to the container.
builder.Services.ConfigureInjection(builder.Configuration);

builder.Services
    .AddControllers()
    .AddControllerOptions();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddApiVersioningWithExplorer();
    

var app = builder.Build();
app.Services.MigrateDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseClaimsSwagger();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/api/health");
app.UseCustomExceptionHandler();

app.Run();

public partial class Program { }
