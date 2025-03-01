using HiQPdf;
using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using System.Linq;
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
                List<string> TestIds =  testId.Split(',').ToList();
                HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
                htmlToPdfConverter.SerialNumber = "YCgJMTAE-BiwJAhIB-EhlWTlBA-UEBRQFBA-U1FOUVJO-WVlZWQ==";

                htmlToPdfConverter.Document.Margins = new PdfMargins(20);
                htmlToPdfConverter.Document.Margins.Top = 100;// 20pt margins (right, bottom, and top)
                htmlToPdfConverter.Document.PageSize = PdfPageSize.A4;   // A4 page size
                htmlToPdfConverter.Document.PageOrientation = PdfPageOrientation.Portrait;

                var headerHeight = 0; 
                int LeftMargin = 30;
                var reportdata = (from tb in db.tnx_Booking
                                  join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                                  join tho in db.tnx_Observations_Histo on tbi.id equals tho.testId
                                  join cm in db.centreMaster on tb.centreId equals cm.centreId
                                  join dr in db.doctorReferalMaster on tb.refID1 equals dr.doctorId
                                  join tm in db.titleMaster on tb.title_id equals tm.id
                                  where testId.Contains(tbi.id.ToString())
                                  select new
                                  {
                                     VisitNo= tb.workOrderId, PatientName=string.Concat(tm.title," ",tb.name),
                                      Age=string.Concat(tb.ageYear," Y ",tb.ageMonth," M ",tb.ageDay,"D/",tb.gender),
                                      tb.patientId,clientName=cm.companyName,ClientCode=cm.centrecode,
                                      RefDoctor= dr.doctorName,tbi.departmentName,tbi.investigationName,tbi.itemId, 
                                      Registrationdate= tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
                                      CollectionDate = tbi.sampleCollectionDate.HasValue ? tbi.sampleCollectionDate.Value.ToString("yyyy-MMM-dd hh:mm tt"): "",
                                      RecievedDate = tbi.sampleReceiveDate.HasValue? tbi.sampleReceiveDate.Value.ToString("yyyy-MMM-dd hh:mm tt"): "",
                                      ApprovedDate = tbi.approvedDate.HasValue? tbi.approvedDate.Value.ToString("yyyy-MMM-dd hh:mm tt"): "",
                                      tho.biospyNumber,tho.blockKeys,tho.clinicalHistory,tho.comment,tho.finalImpression,
                                      tho.typesFixativeUsed,tho.specimen,tho.gross,tho.microscopy
                                  }).ToList();

                string htmlContent = db.labReportHeader.Where(l => l.isActive == 1).Select(l => l.headerCSS.ToString()).FirstOrDefault();
                StringBuilder sb = new StringBuilder();
                sb.Append("<table>");
                var deptname = "";
                var testname = "";
                foreach (var item in reportdata)
                {
                    if (deptname != item.departmentName)
                    {
                        deptname = item.departmentName;
                        sb.Append("<tr><td colspan=2>" + item.departmentName + "</td></tr>");
                    }
                    if (testname != item.investigationName)
                    {
                        testname=item.investigationName;
                        sb.Append("<tr><td colspan=2 style='Border:1px'>" + item.investigationName + "</td></tr>");
                    }
                    sb.Append("<tr><td>Specimen: </td><td>"+item.specimen +"</td></tr>");
                    sb.Append("<tr><td>Biopsy Number: </td><td>" + item.biospyNumber + "</td></tr>");
                    sb.Append("<tr><td colspan=2>Biopsy Number: </td></tr>");


                }
                sb.Append("</table>");
                htmlContent = string.Concat(htmlContent, sb.ToString()); 

                byte[] pdfBuffer = htmlToPdfConverter.ConvertHtmlToMemory(htmlContent, null);

                return pdfBuffer;
            }
            catch (Exception ex)
            {
                return new byte[0];
            }
        }


        public byte[] GetMicroReport(string testId)
        {
            try
            {
                HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
                htmlToPdfConverter.SerialNumber = "YCgJMTAE-BiwJAhIB-EhlWTlBA-UEBRQFBA-U1FOUVJO-WVlZWQ==";

                htmlToPdfConverter.Document.Margins = new PdfMargins(20); // 20pt margins
                htmlToPdfConverter.Document.PageSize = PdfPageSize.A4;   // A4 page size
                htmlToPdfConverter.Document.PageOrientation = PdfPageOrientation.Portrait;

                string htmlContent = db.labReportHeader.Where(l => l.isActive == 1).Select(l => l.headerCSS.ToString()).FirstOrDefault();

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
