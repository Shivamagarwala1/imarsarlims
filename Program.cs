
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using iMARSARLIMS;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.NewtonsoftJson;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Services;
using Serilog;
namespace iMARSARLIMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var logsDirectory = Path.Combine("Error Logs", DateTime.Now.ToString("yyyy-MM-dd"));

            // Ensure the directory exists
            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console() // Optional: Log to the console
                .WriteTo.File(
                    path: Path.Combine(logsDirectory, "error-log.txt"), // File inside the daily folder
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error, // Log errors and above
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}" // Custom format
                )
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder => builder
                        .WithOrigins("http://lims.imarsar.com:8083")  // Allow the React frontend origin
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers()
    .AddNewtonsoftJson(p => p.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
    .AddOData(options => options
        .Expand()
        .Select()
        .OrderBy()
        .Filter()
        .SetMaxTop(1000)
        .Count())
    .AddODataNewtonsoftJson();



            builder.Services.AddDbContextPool<ContextClass>(options =>
            {
                options.UseMySQL(builder.Configuration.GetConnectionString("ConnStr"));
            });

            builder.Services.AddScoped<Itnx_BookingPatientServices, tnx_BookingPatientServices>();
            builder.Services.AddScoped<MySql_Function_Services, MySql_Function_Services>();
            builder.Services.AddScoped<Itnx_BookingItemServices, tnx_BookingItemServices>();
            builder.Services.AddScoped<IempMasterServices, empMasterServices>();
            builder.Services.AddScoped<IroleMenuAccessServices, roleMenuAccessServices>();
            builder.Services.AddScoped<IcentreMasterServices, centreMasterServices>();
            builder.Services.AddScoped<IitemMasterServices, itemMasterServices>();
            builder.Services.AddScoped<MySql_Procedure_Services, MySql_Procedure_Services>();
            builder.Services.AddScoped<IitemObservationMappingServices, itemObservationMappingServices>();
            builder.Services.AddScoped<IrateTypeWiseRateListServices, rateTypeWiseRateListServices>();
            builder.Services.AddSingleton<OpenAIService>();

            var app = builder.Build();

           
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Unhandled exception occurred.");
                    throw;
                }
            });

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("AllowFrontend");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
