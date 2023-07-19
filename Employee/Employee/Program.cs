using Common.Extenstions;
using Common.Settings;
using Employee.Model;
using Employee.Repository;
using Employee.Repository.Interface;


namespace Employee
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            //Use the Extension to add mongo DB
            builder.Services.AddMondoDb(builder.Configuration);

            // Add services to the container.
            builder.Services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(Program));

            //Add Staff Repository Service
            builder.Services.AddTransient<IStaffRepository, StaffRepository>();

            //Add General Repository with Extension   
            builder.Services.AddRepository<Staff>("staffs");

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