
using DinkToPdf.Contracts;
using DinkToPdf;
using iMARSARLIMS;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.NewtonsoftJson;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using System.Text;


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
// Register DinkToPdf
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));


builder.Services.AddCors(opttions =>
{
    opttions.AddPolicy("AllowAllOrgiins", builder =>
    {
        builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});

//.WithOrigins("http://lims.imarsar.com:8083")
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
builder.Services.AddScoped<IObservationMasterServices, ObservationMasterServices>();
builder.Services.AddScoped<IobservationReferenceRangesServices, observationReferenceRangesServices>();
builder.Services.AddScoped<ICommentInterpatationservices, CommentInterpatationservices>();
builder.Services.AddScoped<IMenuMasterServices, MenuMasterServices>();
builder.Services.AddSingleton<OpenAIService>();
builder.Services.AddSingleton<JwtTokenGenrator>();
builder.Services.AddScoped<IPatientReportServices, PatientReportServices>();
builder.Services.AddScoped<ILocationsServices, LocationsServices>();
builder.Services.AddScoped<IitemTemplateServices, itemTemplateServices>();
builder.Services.AddScoped<IdoctorApprovalmasterservices, doctorApprovalmasterservices>();
builder.Services.AddScoped<ImachineMasterServices, machineMasterServices>();
builder.Services.AddScoped<IhelpMenuMasterServices, helpMenuMasterServices>();
builder.Services.AddScoped<IFormulaMasterServices, FormulaMasterServices>();
builder.Services.AddScoped<IorganismAntibioticMasterServices, organismAntibioticMasterServices>();
builder.Services.AddScoped<IorganismAntibioticTagMasterServices, organismAntibioticTagMasterServices>();
builder.Services.AddScoped<IrateTypeMasterServices, rateTypeMasterServices>();
builder.Services.AddScoped<IlabDepartmentServices, labDepartmentServices>();
builder.Services.AddScoped<IitemOutSourceServices, itemOutSourceServices>();
builder.Services.AddScoped<IitemOutHouseServices, itemOutHouseServices>();
builder.Services.AddScoped<IdegreeMasterServices, degreeMasterServices>();
builder.Services.AddScoped<IdesignationMasterServices, designationMasterServices>();
builder.Services.AddScoped<IdiscountReasonMasterServices, discountReasonMasterServices>();
builder.Services.AddScoped<IsampleRejectionReasonServices, sampleRejectionReasonServices>();
builder.Services.AddScoped<ItitleMasterServices, titleMasterServices>();
builder.Services.AddScoped<IdocumentTypeMasterServices, documentTypeMasterServices>();
builder.Services.AddScoped<Ibank_masterServices, bank_masterServices>();
builder.Services.AddHttpClient();
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
app.UseCors("AllowAllOrgiins");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

