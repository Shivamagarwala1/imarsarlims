using iMARSARLIMS;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Interface.appointment;
using iMARSARLIMS.Interface.Sales;
using iMARSARLIMS.Interface.Store;
using iMARSARLIMS.Interface.SupportTicket;
using iMARSARLIMS.Services;
using iMARSARLIMS.Services.Appointment;
using iMARSARLIMS.Services.Sales;
using iMARSARLIMS.Services.Store;
using iMARSARLIMS.Services.SupportTicket;
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
//builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

//builder.Services.AddSignalR();
//builder.Services.AddCors(opttions =>
//{
//    opttions.AddPolicy("AllowAllOrgiins", builder =>
//    {
//        builder
//                .AllowAnyOrigin()
//                .AllowAnyHeader()
//                .AllowAnyMethod();
//});
////});
//.WithOrigins("http://lims.imarsar.com:8083")

// Define specific origins (Replace these with your actual allowed origins)
string[] allowedOrigins = new string[] { "https://imarsar.com:8085", "http://localhost:5268", "https://localhost:44345" };

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrgiins", builder =>
    {
        builder
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((hosts) => true);
    });
});

builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
.AddNewtonsoftJson(p =>
{
    p.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    p.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
    p.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
})
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
builder.Services.AddScoped<IchatGroupServices, chatGroupServices>();
builder.Services.AddScoped<IchatMessageServices, chatMessageServices>();
builder.Services.AddScoped<IdoctorReferalServices, doctorReferalServices>();
builder.Services.AddScoped<IcentreInvoiceServices, centreInvoiceServices>();
builder.Services.AddScoped<ICentrePaymentServices, CentrePaymentServices>();
builder.Services.AddScoped<ItnxInvestigationRemarksService, tnxInvestigationRemarksService>();
builder.Services.AddScoped<ItnxBookingServices, tnxBookingServices>();
builder.Services.AddScoped<Ihistoreportservices, histoReportServices>();
builder.Services.AddScoped<ItnxInvestigationAttachmentService, tnxInvestigationAttachmentService>();
builder.Services.AddScoped<IitemObservation_isnablService, itemObservation_isnablService>();
builder.Services.AddScoped<IlabUniversalMasterServices, labUniversalMasterServices>();
builder.Services.AddScoped<ItatMasterServices, tatMasterServices>();
builder.Services.AddScoped<Itnx_OutsourceDetailServices, tnx_OutsourceDetailServices>();
builder.Services.AddScoped<ICentreCertificateServices, CentreCertificateServices>();
builder.Services.AddScoped<Itnx_testcommentServices, tnx_testcommentServices>();
builder.Services.AddScoped<IinvestigationUDServices, investigationUDServices >();
builder.Services.AddScoped<ImachineRerunTestDetailServices, machineRerunTestDetailServices>();
builder.Services.AddScoped<IItemMasterStoreServices, ItemMasterStoreServices>();
builder.Services.AddScoped<IindentServices, indentServices>();
builder.Services.AddScoped<IMarketingDashBoardServices, MarketingDashBoardServices>();
builder.Services.AddScoped<IrouteMasterServices, routeMasterServices>();
builder.Services.AddScoped<ItimeSlotMasterServices, timeSlotMasterServices>();
builder.Services.AddScoped<IappointmentBookingServices, appointmentBookingServices>();
builder.Services.AddScoped<IdoctorShareMasterServices, doctorShareMasterServices>();
builder.Services.AddScoped<IsupportTicketServices, supportTicketServices>();
builder.Services.AddScoped<ISalesEmployeeTaggingService, SalesEmployeeTaggingService>();
builder.Services.AddHttpClient();
builder.Services.AddTransient<RazorpayService>();
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
app.MapHub<ChatHub>("/chatHub");
app.Run();

