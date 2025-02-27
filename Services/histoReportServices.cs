using HiQPdf;
using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using System.Text;

namespace iMARSARLIMS.Services
{
    public class histoReportServices : Ihistoreportservices
    {
        private readonly ContextClass db;
        public histoReportServices(ContextClass context, ILogger<BaseController<bank_master>> logger)
        {
            db = context;
        }
        public byte[] GetHistoReport(string testId)
        {
            try
            {
                HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
                htmlToPdfConverter.SerialNumber = "YCgJMTAE-BiwJAhIB-EhlWTlBA-UEBRQFBA-U1FOUVJO-WVlZWQ==";

                htmlToPdfConverter.Document.Margins = new PdfMargins(20); // 20pt margins
                htmlToPdfConverter.Document.PageSize = PdfPageSize.A4;   // A4 page size
                htmlToPdfConverter.Document.PageOrientation = PdfPageOrientation.Portrait;
                string htmlContent = "<h1>Welcome to HiQPdf!</h1><p>This is a sample PDF generated with HiQPdf in .NET Core.</p>";
                
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
                return pdfBuffer;
            }
            catch (Exception ex)
            {
                return new byte[0];
            }
        }
    }
}
