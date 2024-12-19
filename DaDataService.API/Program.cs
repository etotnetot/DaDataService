using AutoMapper;
using DaDataService.API.Middlewares;
using DaDataService.BLL.Mappers;
using DaDataService.BLL.Services;
using DaDataService.Shared.Models;
using Microsoft.Extensions.Options;
using Serilog;

namespace DaDataService.API
{
    internal class Program
    {
        static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("appsettings.json");
            builder.Services.Configure<DaDataServiceOptions>(builder.Configuration.GetSection("DaData"));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());

            builder.Services.AddAutoMapper(typeof(AddressProfile));
            builder.Services.AddHttpClient<AddressStandardizationHttpClient>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["DaData:BaseUrl"]);
            });

            builder.Services.AddTransient<IAddressStandardizationService>(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(nameof(AddressStandardizationHttpClient));
                var dataServiceOptions = provider.GetRequiredService<IOptions<DaDataServiceOptions>>();
                var addressMapper = provider.GetRequiredService<IMapper>();

                return new AddressStandardizationService(httpClient, dataServiceOptions, addressMapper);
            });

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy
                (
                    policy => policy.WithOrigins("http://cleaner.dadata.ru/")
                );
            });
        }

        static void ConfigureMiddleware(WebApplication app)
        {
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
        }

        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);

            var app = builder.Build();
            ConfigureMiddleware(app);

            app.Run();
        }
    }
}