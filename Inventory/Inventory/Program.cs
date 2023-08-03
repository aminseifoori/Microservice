
using Common.Extenstions;
using Common.Settings;
using Inventory.Clients;
using Inventory.Model;
using Inventory.Setting;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Timeout;

namespace Inventory
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMondoDb(builder.Configuration);
            builder.Services.AddRepository<AssignedInventory>("assignedInventories");

            builder.Services.AddHttpClient<EmployeeClient>(client =>
            {
                var rootBase = builder.Configuration.GetSection(nameof(EmployeeClientService)).Get<EmployeeClientService>();
                client.BaseAddress = new Uri(rootBase.BaseRoot);
            })
            .AddTransientHttpErrorPolicy(option => option.Or<TimeoutRejectedException>().WaitAndRetryAsync(
                 5,
                 retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                ))
            .AddTransientHttpErrorPolicy(option => option.Or<TimeoutRejectedException>().CircuitBreakerAsync(
                3,
                TimeSpan.FromSeconds(15)
                ))
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(2)); //Set timeout for http request
            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(Program));

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
        }
    }
}