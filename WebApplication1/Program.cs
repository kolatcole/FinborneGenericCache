using FinborneGenericCache.Core;
using FinborneGenericCache.Interface;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Log.Logger = new LoggerConfiguration()
.WriteTo.Console()
.CreateLogger();

builder.Services.AddLogging(builder =>
{
    builder.AddSerilog(dispose: true);
});
builder.Services.AddSingleton<IGenericCache<int,string>, GenericCache<int, string>>(provider =>
{
    var cacheConfig = new GenericCacheConfig { Limit = 40 };
    return new GenericCache<int, string>(cacheConfig, provider.GetService<ILogger<GenericCache<int, string>>>());
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
