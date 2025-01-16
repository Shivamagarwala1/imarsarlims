
using System.Text;
using HiQPdf;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using iMARSARLIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class empMasterController : BaseController<empMaster>
    {
        private readonly ContextClass db;
        private readonly IempMasterServices _empMasterServices;
        private readonly ILogger<BaseController<empMaster>> _logger;
        private readonly OpenAIService _openAIService;

        public empMasterController(ContextClass context, ILogger<BaseController<empMaster>> logger, IempMasterServices empMasterServices, OpenAIService openAIService) : base(context, logger)
        {
            db = context;
            this._empMasterServices = empMasterServices;
            _logger = logger;
            _openAIService = openAIService;
        }
        protected override IQueryable<empMaster> DbSet => db.empMaster.AsNoTracking().OrderBy(o => o.empId);


        [HttpPost("UpdatePassword")]
        public async Task<ActionResult<ServiceStatusResponseModel>> UpdatePassword(int Employeeid, string Password)
        {
            try
            {
                var result = await _empMasterServices.UpdatePassword(Employeeid, Password);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateEmployeeStatus")]
        public async Task<ActionResult<ServiceStatusResponseModel>> UpdateEmployeeStatus(int EmplyeeId, bool status, int UserId)
        {
            if (EmplyeeId == 0)
                return BadRequest("Invalid Employee ID.");

            try
            {
                var result = await _empMasterServices.UpdateEmployeeStatus(EmplyeeId,status,UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("EmployeeWiseCentre")]
        public async Task<ActionResult<ServiceStatusResponseModel>> EmployeeWiseCentre(int EmplyeeId)
        {
            if (EmplyeeId == 0)
                return BadRequest("Invalid Employee ID.");

            try
            {
                var result = await _empMasterServices.EmployeeWiseCentre(EmplyeeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("EmployeeWiseRole")]
        public async Task<ActionResult<ServiceStatusResponseModel>> EmployeeWiseRole(int EmplyeeId)
        {
            if (EmplyeeId == 0)
                return BadRequest("Invalid Role ID.");

            try
            {
                var result = await _empMasterServices.EmployeeWiseRole(EmplyeeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("EmployeeWiseMenu")]
        public async Task<ActionResult<ServiceStatusResponseModel>> EmployeeWiseMenu(String EmplyeeId,String RoleId,string CentreId)
        {
            if (EmplyeeId == "0" || EmplyeeId == "")
                return BadRequest("Invalid Employee ID.");
            if (RoleId == "" || RoleId == "0")
                return BadRequest("Invalid Role ID.");
            if (CentreId == "0"|| CentreId=="")
                return BadRequest("Invalid Role ID.");

            try
            {
                var result = await _empMasterServices.EmployeeWiseMenu(EmplyeeId, RoleId,CentreId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("GetAllMenu")]
        public async Task<ActionResult<ServiceStatusResponseModel>> GetAllMenu()
        {
            try
            {
                var result = await _empMasterServices.GetAllMenu();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("SaveEmployee")]
        public async Task<ActionResult<ServiceStatusResponseModel>> SaveEmployee(empMaster empmaster)
        {
            try
            {
                var result = await _empMasterServices.SaveEmployee(empmaster);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetEmployeeData")]
        public async Task<ServiceStatusResponseModel> GetEmployeeData(int EmpId)
        {
            try
            {
                var result = await db.empMaster.Where(p => p.empId == EmpId)
                             .Include(p => p.addEmpCentreAccess).Include(p => p.addEmpDepartmentAccess).Include(p => p.addEmpRoleAccess).FirstOrDefaultAsync();
                if(result == null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "Please enter correct EmpId"
                    };
                }
                return new ServiceStatusResponseModel
                {
                    Success=true,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        [HttpPost("UploadDocument")]
        
        public async Task<ActionResult<ServiceStatusResponseModel>> UploadDocument(IFormFile Document)
        {
            try
            {
                var result = await _empMasterServices.UploadDocument(Document);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("DownloadImage")]
        public async Task<ActionResult<ServiceStatusResponseModel>> DownloadImage(int emplpyeeid)
        {
            try
            {
                var result = await _empMasterServices.DownloadImage(emplpyeeid);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("generate-hiqpdf")]
        public IActionResult GenerateHiqpdf()
        {
            try
            {
                //LGRFfXxI-SmBFTl5N-XlUdHgIc-DB0MHQwV-FB0YDB8d-Ah0eAhUV-FRU=
                // Initialize the PDF document
                HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
                htmlToPdfConverter.SerialNumber = "YCgJMTAE-BiwJAhIB-EhlWTlBA-UEBRQFBA-U1FOUVJO-WVlZWQ==";


                // Optional settings
                htmlToPdfConverter.Document.Margins = new PdfMargins(20); // 20pt margins
                htmlToPdfConverter.Document.PageSize = PdfPageSize.A4;   // A4 page size
                htmlToPdfConverter.Document.PageOrientation = PdfPageOrientation.Portrait;

                // Convert HTML string to PDF
                string htmlContent = "<h1>Welcome to HiQPdf!</h1><p>This is a sample PDF generated with HiQPdf in .NET Core.</p>";
               // var dataGenerator = new DummyDataGenerator();
               // var data = dataGenerator.GenerateDummyData(100);
                StringBuilder sb = new StringBuilder();
                sb.Append("<table border='1' style='border-collapse: collapse; width: 100%;'>"); // Add border and styles for clarity
                sb.Append("<tr><th>ID</th><th>Name</th><th>Age</th></tr>"); // Header row
               // foreach (var item in data)
               // {
               //     sb.Append("<tr>");
               //     sb.Append($"<td style='color:red'>{item.Id}</td>");
               //     sb.Append($"<td style='color:green'>{item.Name}</td>");
               //     sb.Append($"<td style='color:blue'>{item.Age}</td>");
               //     sb.Append("</tr>");
               // }
                sb.Append("</table>");
                htmlContent = string.Concat(htmlContent, sb.ToString());

                  byte[] pdfBuffer = htmlToPdfConverter.ConvertHtmlToMemory(htmlContent, null);

                // Return the PDF as a downloadable file
                return File(pdfBuffer, "application/pdf", "Sample.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error generating PDF: {ex.Message}");
            }
        }

        [HttpGet("error")]
        public IActionResult GenerateError()
        {
            try
            {
                // Simulate an exception
                throw new Exception("This is a test error.");
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask(String ask)
        {
            try
            {
                var response = await _openAIService.GetChatResponseAsync(ask);
                return Ok(new { response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpPost("ask_google")]
        public async Task<IActionResult> Ask_Google(String ask)
        {
            try
            {
                // Set the Google Application Credentials (replace with your actual path)
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"shivam-map-api-demo-bc5df779fa02.json");

                // Create a session client
                var sessionClient = Google.Cloud.Dialogflow.V2.SessionsClient.Create();

                // Define the session name (replace PROJECT_ID and SESSION_ID)
                string projectId = "shivam-map-api-demo";
                string sessionId = Guid.NewGuid().ToString(); // Use a unique session ID for each user
                var sessionName = Google.Cloud.Dialogflow.V2.SessionName.FromProjectSession(projectId, sessionId);

                // Create a query input
                var queryInput = new Google.Cloud.Dialogflow.V2.QueryInput
                {
                    Text = new Google.Cloud.Dialogflow.V2.TextInput
                    {
                        Text = ask, // User query
                        LanguageCode = "en-US" // Language code
                    }
                };

                // Detect intent
                var response = sessionClient.DetectIntent(sessionName, queryInput);


                return Ok(new { response.QueryResult.FulfillmentText });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}
