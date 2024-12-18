
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using iMARSARLIMS;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.NewtonsoftJson;
using System.Text;

using Microsoft.OpenApi.Models;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKeys = [new SymmetricSecurityKey(secretKey)]
    };
});
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(C =>
    {
        C.SwaggerDoc("v1", new OpenApiInfo { Title = "LIMS BE", Version = "v1" });
        C.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the bearer scheme. Example : \" Authorization: Bearer {token}\"",
        });
        C.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                }, new List<string>()
            }
        });
    });
// Configure Serilog
var logsDirectory = Path.Combine("Error Logs", DateTime.Now.ToString("yyyy-MM-dd"));

// Ensure the directory exists
if (!Directory.Exists(logsDirectory))
{
    Directory.CreateDirectory(logsDirectory);
}
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
builder.Services.AddSingleton<JwtTokenGenrator>();

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

app.UseSwagger(option =>
{
    option.RouteTemplate = "/swagger/{documentName}/swagger.json";
});
app.UseSwaggerUI(option =>
{
    option.SwaggerEndpoint("/swagger/v1/swagger.json", "LIMS BE API");
    option.RoutePrefix = "swagger";
});
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

