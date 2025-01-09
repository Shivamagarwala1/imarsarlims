using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Linq;

namespace iMARSARLIMS.Services
{
    public class PatientReportServices : IPatientReportServices
    {
        private readonly ContextClass db;
        public PatientReportServices(ContextClass context, ILogger<BaseController<tnx_BookingPatient>> logger)
        {

            db = context;
        }
        public byte[] GetPatientReport(string TestId)
        {
            var testIds = TestId.Split(',').Select(int.Parse).ToList();
            var Reportdata = (from tb in db.tnx_Booking
                               join tbp in db.tnx_BookingPatient on tb.patientId equals tbp.patientId
                               join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                               join tos in db.tnx_Observations on tbi.id equals tos.testId
                               join tm in db.titleMaster on tb.title_id equals tm.id
                               join cm in db.centreMaster on tb.centreId equals cm.id
                               where testIds.Contains(tbi.id) && tos.showInReport == 1
                               select new
                               {
                                   tb.workOrderId,
                                   BookingDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
                                   tm.title,
                                   tb.name,
                                   Age= tb.ageYear+"Y "+tb.ageMonth+"M "+tb.ageDay+"D",
                                   cm.companyName,
                                   CentreAddress = cm.address,
                                   cm.centrecode,
                                   CentreMobile = cm.mobileNo,
                                   tbp.mobileNo,
                                   tb.gender,
                                   tbi.barcodeNo,
                                   tbi.departmentName,
                                   tbi.investigationName,
                                   tos.observationName,
                                   tos.value,
                                   tos.min,
                                   tos.max,
                                   tos.minCritical,
                                   tos.maxCritical,
                                   tos.unit,
                                   tos.testMethod,
                                   tos.displayReading,
                                   tos.flag,
                                   tos.isHeader,
                                   tos.isBold,
                                   SampleCollectionDate = tbi.sampleCollectionDate.ToString("yyyy-MMM-dd hh:mm tt"),
                                   ResultDate = tbi.resultDate.ToString("yyyy-MMM-dd hh:mm tt")
                               }).ToList();

            if (Reportdata.Count > 0)
            {
                QuestPDF.Settings.License = LicenseType.Community;

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);

                        page.MarginTop(3.5f, Unit.Centimetre);
                        page.MarginLeft(0.5f, Unit.Centimetre);
                        page.MarginRight(0.5f, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontFamily("Arial"));
                        page.DefaultTextStyle(x => x.FontSize(10));

                        var data = "";

                        page.Header()
                        .Column(column =>
                        {
                            column.Item()
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                    });
                                    table.Cell().BorderLeft(1.0f, Unit.Point).BorderTop(1.0f, Unit.Point).Text(" Patient Name").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).BorderTop(1.0f, Unit.Point).Text(": "+ Reportdata[0].name).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().BorderLeft(1.0f, Unit.Point).BorderTop(1.0f, Unit.Point).Text(" Collected").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).BorderRight(1.0f, Unit.Point).BorderTop(1.0f, Unit.Point).Text(": "+ Reportdata[0].SampleCollectionDate).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().BorderLeft(1.0f, Unit.Point).Text(" Age/Gender").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text(": " + Reportdata[0].Age +"/"+ ": " + Reportdata[0].gender).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().BorderLeft(1.0f, Unit.Point).Text(" Received").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).BorderRight(1.0f, Unit.Point).Text(": " + Reportdata[0].SampleCollectionDate).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().BorderLeft(1.0f, Unit.Point).Text(" Ref Doctor").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text("").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().BorderLeft(1.0f, Unit.Point).Text(" Reported").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).BorderRight(1.0f, Unit.Point).Text(": " + Reportdata[0].ResultDate).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().BorderLeft(1.0f, Unit.Point).Text(" Barcode No").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).Text(": " + Reportdata[0].barcodeNo).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().BorderLeft(1.0f, Unit.Point).Text(" Client Code").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).BorderRight(1.0f, Unit.Point).Text(": " + Reportdata[0].centrecode).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().BorderLeft(1.0f, Unit.Point).BorderBottom(1.0f, Unit.Point).Text(" Visit Id").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).BorderBottom(1.0f, Unit.Point).Text(": " + Reportdata[0].workOrderId).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().BorderLeft(1.0f, Unit.Point).BorderBottom(1.0f, Unit.Point).Text(" Client Name").Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(2).BorderBottom(1.0f, Unit.Point).BorderRight(1.0f, Unit.Point).Text(": " + Reportdata[0].companyName).Style(TextStyle.Default.FontSize(10));
                                    table.Cell().ColumnSpan(6).Text("").Style(TextStyle.Default.FontSize(6));
                                });
                            column.Item()
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                    });
                                   
                                    var departmentname = "";

                                    foreach (var item in Reportdata)
                                    {
                                        if (item.departmentName != departmentname && departmentname!="")
                                        {
                                            table.Cell().ColumnSpan(11).Text("").Style(TextStyle.Default.FontSize(10)).AlignCenter();
                                            table.Cell().ColumnSpan(11).Text("").Style(TextStyle.Default.FontSize(10)).AlignCenter();
                                        }

                                        if (item.departmentName != departmentname)
                                        {
                                            table.Cell().ColumnSpan(3).Height(0.5f,Unit.Centimetre).Border(0.4f, Unit.Point).Text("Test Name").Style(TextStyle.Default.FontSize(10).Bold());
                                            table.Cell().ColumnSpan(2).Height(0.5f,Unit.Centimetre).Border(0.4f, Unit.Point).Text("Result").Style(TextStyle.Default.FontSize(10).Bold());
                                            table.Cell().ColumnSpan(1).Height(0.5f,Unit.Centimetre).Border(0.4f, Unit.Point).Text("Unit").Style(TextStyle.Default.FontSize(10).Bold());
                                            table.Cell().ColumnSpan(3).Height(0.5f,Unit.Centimetre).Border(0.4f, Unit.Point).Text("Bio. Ref. Range").Style(TextStyle.Default.FontSize(10).Bold());
                                            table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Border(0.4f, Unit.Point).Text("Method Name").Style(TextStyle.Default.FontSize(10).Bold());
                                            table.Cell().ColumnSpan(11).Height(0.5f,Unit.Centimetre).Border(0.4f, Unit.Point).Text(item.departmentName).Style(TextStyle.Default.FontSize(10).Bold()).AlignCenter();
                                            table.Cell().ColumnSpan(11).Height(0.5f,Unit.Centimetre).Text("").Style(TextStyle.Default.FontSize(10)).AlignCenter();
                                            departmentname = item.departmentName;
                                        }
                                        if (item.isHeader == 1)
                                        {
                                            table.Cell().ColumnSpan(11).Height(0.5f, Unit.Centimetre).Border(0.4f, Unit.Point).Text(item.investigationName).Style(TextStyle.Default.FontSize(10)).AlignLeft();
                                        }
                                        else
                                        {
                                            table.Cell().ColumnSpan(3).Height(0.5f,Unit.Centimetre).Border(0.4f, Unit.Point).Text(item.observationName).Style(TextStyle.Default.FontSize(10)); // Service Name
                                            table.Cell().ColumnSpan(2).Height(0.5f,Unit.Centimetre).Border(0.4f, Unit.Point).Text(item.value).Style(TextStyle.Default.FontSize(10));   // Department
                                            table.Cell().ColumnSpan(1).Height(0.5f,Unit.Centimetre).Border(0.4f, Unit.Point).Text(item.unit).Style(TextStyle.Default.FontSize(10)); // Rate
                                            table.Cell().ColumnSpan(3).Height(0.5f,Unit.Centimetre).Border(0.4f, Unit.Point).Text(item.displayReading).Style(TextStyle.Default.FontSize(10)); // Discount
                                            table.Cell().ColumnSpan(2).Height(0.5f,Unit.Centimetre).Border(0.4f, Unit.Point).Text(item.testMethod).Style(TextStyle.Default.FontSize(10)); // Total
                                        }
                                       
                                    }
                                });
                        });
                        page.Footer()
                           .Column(column =>
                           {
                               column.Item()
                                   .AlignCenter()
                                   .Text(text =>
                                   {
                                       text.DefaultTextStyle(x => x.FontSize(8));
                                       text.CurrentPageNumber();
                                       text.Span(" of ");
                                       text.TotalPages();
                                   });
                           });

                    });
                });
                byte[] pdfBytes = document.GeneratePdf();
                return pdfBytes;
            }
            else
            {
                byte[] pdfbyte = [];
                return pdfbyte;
            }
        }
    }
}
