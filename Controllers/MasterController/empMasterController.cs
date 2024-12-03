using HiQPdf;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class empMasterController : BaseController<empMaster>
    {
        private readonly ContextClass db;
        private readonly IempMasterServices _empMasterServices;


        public empMasterController(ContextClass context, ILogger<BaseController<empMaster>> logger, IempMasterServices empMasterServices) : base(context, logger)
        {
            db = context;
            this._empMasterServices = empMasterServices;
        }
        protected override IQueryable<empMaster> DbSet => db.empMaster.AsNoTracking().OrderBy(o => o.id);


        [HttpPost("Login")]
        public async Task<ActionResult<List<LoginResponseModel>>> EmpLogin(LoginRequestModel loginRequestModel)
        {
            try
            {
                var result = await _empMasterServices.EmpLogin(loginRequestModel);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                var dataGenerator = new DummyDataGenerator();
                var data = dataGenerator.GenerateDummyData(100);
                StringBuilder sb = new StringBuilder();
                sb.Append("<table border='1' style='border-collapse: collapse; width: 100%;'>"); // Add border and styles for clarity
                sb.Append("<tr><th>ID</th><th>Name</th><th>Age</th></tr>"); // Header row
                foreach (var item in data)
                {
                    sb.Append("<tr>");
                    sb.Append($"<td style='color:red'>{item.Id}</td>");
                    sb.Append($"<td style='color:green'>{item.Name}</td>");
                    sb.Append($"<td style='color:blue'>{item.Age}</td>");
                    sb.Append("</tr>");
                }
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
        [HttpGet("generate-quest")]
        public FileStreamResult GetPDF()
        {
            QuestPDF.Settings.License = LicenseType.Community;
            // Create the PDF document using QuestPDF
            Document document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White); // Optional, can be omitted if the default white background is fine
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header()
                        .Text("Hello PDF!")
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                    var dataGenerator = new DummyDataGenerator();
                    var data = dataGenerator.GenerateDummyData(100);

                    page.Content().Column(column =>
                    {
                        column.Spacing(10);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);  // Column for ID
                                columns.RelativeColumn();    // Column for Name
                                columns.RelativeColumn();    // Column for Age
                            });

                            foreach (var item in data)
                            {
                                table.Cell().Text(item.Id.ToString()).Style(TextStyle.Default.FontSize(12));
                                table.Cell().Text(item.Name).Style(TextStyle.Default.FontSize(16));
                                table.Cell().Text(item.Age.ToString()).Style(TextStyle.Default.FontSize(20));
                            }
                        });
                    });
                    page.Footer()
                    .AlignCenter()
                        .Text(text =>
                        {
                            text.DefaultTextStyle(x => x.FontSize(15));
                            text.Span("Page number").FontSize(10);
                            text.CurrentPageNumber();
                            text.Span(" out of ");
                            text.TotalPages();
                        });
                });
            });

            // Generate the PDF as a byte array
            byte[] pdfBytes = document.GeneratePdf();

            // Create a MemoryStream from the PDF bytes
            MemoryStream ms = new MemoryStream(pdfBytes);

            // Return the PDF as a FileStreamResult with MIME type for PDF
            return new FileStreamResult(ms, "application/pdf")
            {
                FileDownloadName = "GeneratedDocument.pdf" // Optional: Name for the downloaded file
            };
        }

        public class Person
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public class DummyDataGenerator
        {
            public List<Person> GenerateDummyData(int count)
            {
                var random = new Random();
                var names = new List<string> { "John", "Jane", "Alice", "Bob", "Charlie", "Diana", "Eve", "Frank" };

                var dummyData = new List<Person>();

                for (int i = 0; i < count; i++)
                {
                    var person = new Person
                    {
                        Id = i + 1,
                        Name = names[random.Next(names.Count)], // Random name from list
                        Age = random.Next(18, 70) // Random age between 18 and 70
                    };

                    dummyData.Add(person);
                }

                return dummyData;
            }
        }

    }
}
