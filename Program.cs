
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using iMARSARLIMS;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.NewtonsoftJson;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Services;
namespace iMARSARLIMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder => builder
                        .WithOrigins("http://lims.imarsar.com:8081")  // Allow the React frontend origin
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
