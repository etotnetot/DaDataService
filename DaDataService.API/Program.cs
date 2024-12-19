using DaDataService.API.Middlewares;
using DaDataService.BLL.Mappers;
using DaDataService.BLL.Services;
using DaDataService.Shared.Models;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.Configure<DaDataServiceOptions>(builder.Configuration.GetSection("DaData"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());

builder.Services.AddAutoMapper(typeof(AddressProfile));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddHttpClient<AddressStandardizationHttpClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["DaData:BaseUrl"]);
});

builder.Services.AddTransient<IAddressStandardizationService>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient(nameof(AddressStandardizationHttpClient));
    var options = provider.GetRequiredService<IOptions<DaDataServiceOptions>>();

    return new AddressStandardizationService(httpClient, options);
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();