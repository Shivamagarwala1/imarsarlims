using System.ComponentModel;
using System.Reflection.PortableExecutable;
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

                    // Header section
                    page.Header()
                        .Text("Hello PDF!")
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                    //// Main content section
                    //page.Content()
                    //    .PaddingVertical(1, Unit.Centimetre)
                    //    .Column(column =>
                    //    {
                    //        column.Spacing(20);

                    //        column.Item().Text("dslknlkdsnvlkd  nfdsnvldn lkfnflkdsnl lkdsfnlkdsnflk lksdnflkdsnf"); // Sample text
                    //        column.Item().Image(Placeholders.Image(200, 100)); // Sample image
                    //    });

                    //// Footer section
                    //page.Footer()
                    //    .AlignCenter()
                    //    .Text(x =>
                    //    {
                    //        x.Span("Page ");
                    //        x.CurrentPageNumber(); // Display current page number
                    //    });

                    //page.Header()
                    //.Row(row =>
                    //{
                    //    row.Spacing(25);
                    //    row.RelativeItem()
                    //        .Column(column =>
                    //        {
                    //            column.Item()
                    //            .Text("My header")
                    //            .SemiBold()
                    //            .FontSize(30)
                    //            .FontColor(Colors.Blue.Medium);

                    //            column.Item()
                    //            .Text("Q&A Table")
                    //            .Underline()
                    //            .FontSize(20)
                    //            .FontColor(Colors.Blue.Medium);
                    //        });
                    //});
                    //page.Content()
                    //.PaddingVertical(25).Table(table =>
                    //{
                    //    table.ColumnsDefinition(columns =>
                    //    {
                    //        // s.no, question,qtype, weightage, sortorder
                    //        columns.ConstantColumn(50);
                    //        columns.RelativeColumn();
                    //        columns.RelativeColumn();
                    //        columns.ConstantColumn(60);
                    //        columns.ConstantColumn(60);
                    //    });
                    //    table.Header(header =>
                    //    {
                    //        header.Cell().Text("S.No");
                    //        header.Cell().AlignRight().Text("Question");
                    //        header.Cell().AlignRight().Text("QType");
                    //        header.Cell().AlignRight().Text("Weightage");
                    //        header.Cell().AlignRight().Text("Sortorder");
                    //    });
                    //    table.Cell().Text("1");
                    //    table.Cell().Text("what is the important aspects of IT?");
                    //    table.Cell().AlignRight().Text("Essay");
                    //    table.Cell().AlignRight().Text("5");
                    //    table.Cell().AlignRight().Text("6");

                    //    table.Cell().Text("2");
                    //    table.Cell().Text("an extension of image file?");
                    //    table.Cell().AlignRight().Text("Choice");
                    //    table.Cell().AlignRight().Text("2");
                    //    table.Cell().AlignRight().Text("1");

                    //    table.Cell().Text("3");
                    //    table.Cell().Text("What are security controls?");
                    //    table.Cell().AlignRight().Text("Essay");
                    //    table.Cell().AlignRight().Text("5");
                    //    table.Cell().AlignRight().Text("7");

                    //    table.Cell().Text("4");
                    //    table.Cell().Text("compliance standard?");
                    //    table.Cell().AlignRight().Text("Choice");
                    //    table.Cell().AlignRight().Text("2");
                    //    table.Cell().AlignRight().Text("2");

                    //    table.Cell().Text("5");
                    //    table.Cell().Text("Risk is the failure in component?");
                    //    table.Cell().AlignRight().Text("Choice");
                    //    table.Cell().AlignRight().Text("2");
                    //    table.Cell().AlignRight().Text("3");

                    //    table.Cell().Text("6");
                    //    table.Cell().Text("Third party tools in software?");
                    //    table.Cell().AlignRight().Text("Essay");
                    //    table.Cell().AlignRight().Text("5");
                    //    table.Cell().AlignRight().Text("5");

                    //    table.Cell().Text("7");
                    //    table.Cell().Text("What are cloud based solutions?");
                    //    table.Cell().AlignRight().Text("Essay");
                    //    table.Cell().AlignRight().Text("5");
                    //    table.Cell().AlignRight().Text("4");
                    //});
                    //page.Footer()
                    //.AlignCenter()
                    //    .Text(text =>
                    //    {
                    //        text.DefaultTextStyle(x => x.FontSize(15));
                    //        text.Span("Page number").FontSize(10);
                    //        text.CurrentPageNumber();
                    //        text.Span(" out of ");
                    //        text.TotalPages();
                    //    });

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
                                table.Cell().Text(item.Id.ToString());
                                table.Cell().Text(item.Name);
                                table.Cell().Text(item.Age.ToString());
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
