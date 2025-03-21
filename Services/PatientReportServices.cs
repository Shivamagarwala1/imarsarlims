using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Transaction;
using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using IronBarCode;
using iMARSARLIMS.Response_Model;
using System.Text;
using Microsoft.Extensions.Primitives;


namespace iMARSARLIMS.Services
{
    public class PatientReportServices : IPatientReportServices
    {


        private readonly ContextClass db;
        public PatientReportServices(ContextClass context, ILogger<BaseController<tnx_BookingPatient>> logger)
        {

            db = context;
        }
        public byte[] GetPatientReportType1(string TestId, int header)
        {

            var testIds = TestId.Split(',').Select(int.Parse).ToList();
            var Reportdata = (from tb in db.tnx_Booking
                              join tbp in db.tnx_BookingPatient on tb.patientId equals tbp.patientId
                              join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                              join tos in db.tnx_Observations on tbi.id equals tos.testId
                              join tm in db.titleMaster on tb.title_id equals tm.id
                              join cm in db.centreMaster on tb.centreId equals cm.centreId
                              where testIds.Contains(tbi.id) && tos.showInReport == 1
                              select new
                              {
                                  tb.workOrderId,
                                  BookingDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
                                  tm.title,
                                  tb.name,
                                  Age = tb.ageYear + "Y " + tb.ageMonth + "M " + tb.ageDay + "D",
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
                                  SampleCollectionDate = tbi.sampleCollectionDate,
                                  ResultDate = tbi.resultDate
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
                        page.Foreground();
                        page.DefaultTextStyle(x => x.FontFamily("TimesNewRoman"));
                        page.DefaultTextStyle(x => x.FontSize(10));
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

                                    table.Cell().ColumnSpan(3).Border(0.8f, Unit.Point).Element(innerTable =>
                                    {
                                        innerTable.Table(subTable =>
                                        {
                                            subTable.ColumnsDefinition(columns =>
                                            {
                                                columns.ConstantColumn(3.3f, Unit.Centimetre);
                                                columns.ConstantColumn(6.0f, Unit.Centimetre);
                                            });
                                            subTable.Cell().Text(" Patient Name").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": " + Reportdata[0].name).Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(" Age/Gender").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": " + Reportdata[0].Age + "/" + ": " + Reportdata[0].gender).Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(" Ref Doctor").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": ").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(" Barcode No").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": " + Reportdata[0].barcodeNo).Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(" Visit Id").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": " + Reportdata[0].workOrderId).Style(TextStyle.Default.FontSize(10));
                                        });
                                    });
                                    table.Cell().ColumnSpan(3).Border(0.8f, Unit.Point).Element(innerTable =>
                                    {
                                        innerTable.Table(subTable =>
                                        {
                                            subTable.ColumnsDefinition(columns =>
                                            {
                                                columns.ConstantColumn(3.3f, Unit.Centimetre);
                                                columns.ConstantColumn(6.0f, Unit.Centimetre);
                                            });
                                            subTable.Cell().Text(" Collected").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": " + Reportdata[0].SampleCollectionDate).Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(" Received").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": " + Reportdata[0].SampleCollectionDate).Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(" Reported").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": " + Reportdata[0].ResultDate).Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(" Client Code").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": " + Reportdata[0].centrecode).Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(" Client Name").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": " + Reportdata[0].companyName).Style(TextStyle.Default.FontSize(10));
                                        });
                                    });

                                    table.Cell().ColumnSpan(6).Text("").Style(TextStyle.Default.FontSize(6));
                                });
                        });

                        page.Content()
                        .Column(column =>
                        {
                            string departmentName = "";

                            foreach (var item in Reportdata)
                            {
                                if (item.departmentName != departmentName && !string.IsNullOrEmpty(departmentName))
                                {
                                    column.Item().Element(e => e.PageBreak()); // Add page break between departments
                                }

                                if (item.departmentName != departmentName)
                                {
                                    column.Item().Table(table =>
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

                                        table.Cell().ColumnSpan(11).Height(0.5f, Unit.Centimetre).Border(0.4f, Unit.Point).Text(item.departmentName).Style(TextStyle.Default.FontSize(10).Bold()).AlignCenter();
                                        //result data Header
                                        table.Cell().ColumnSpan(3).Height(0.5f, Unit.Centimetre).Border(0.4f, Unit.Point).Text(" Test Name").Style(TextStyle.Default.FontSize(10).Bold());
                                        table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Border(0.4f, Unit.Point).Text(" Result").Style(TextStyle.Default.FontSize(10).Bold());
                                        table.Cell().ColumnSpan(1).Height(0.5f, Unit.Centimetre).Border(0.4f, Unit.Point).Text(" Unit").Style(TextStyle.Default.FontSize(10).Bold());
                                        table.Cell().ColumnSpan(3).Height(0.5f, Unit.Centimetre).Border(0.4f, Unit.Point).Text(" Bio. Ref. Range").Style(TextStyle.Default.FontSize(10).Bold());
                                        table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Border(0.4f, Unit.Point).Text(" Method Name").Style(TextStyle.Default.FontSize(10).Bold());
                                        // end here
                                        table.Cell().ColumnSpan(11).Height(0.5f, Unit.Centimetre).Text("").Style(TextStyle.Default.FontSize(10)).AlignCenter();

                                        departmentName = item.departmentName;
                                    });
                                }

                                column.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(3);
                                        columns.RelativeColumn(2);
                                        columns.RelativeColumn(1);
                                        columns.RelativeColumn(3);
                                        columns.RelativeColumn(2);
                                    });

                                    if (item.isHeader == 1)
                                    {
                                        table.Cell().ColumnSpan(5).Height(0.5f, Unit.Centimetre).Border(0.4f, Unit.Point)
                                            .Text(" " + item.investigationName).Style(TextStyle.Default.FontSize(10).Bold()).AlignLeft();
                                    }
                                    else
                                    {
                                        table.Cell().Border(0.4f, Unit.Point).Text(" " + item.observationName).Style(TextStyle.Default.FontSize(10));
                                        if (double.TryParse(item.value, out double numericValue))
                                        {
                                            if (numericValue < item.min || numericValue > item.max)
                                                table.Cell().Border(0.4f, Unit.Point).Text(" " + item.value).Style(TextStyle.Default.FontSize(10).Bold().FontColor(QuestPDF.Infrastructure.Color.FromHex("#B20000"))); // Highlight abnormal values
                                            else
                                                table.Cell().Border(0.4f, Unit.Point).Text(" " + item.value).Style(TextStyle.Default.FontSize(10)); // Normal style for values within the range
                                        }
                                        else
                                        {
                                            table.Cell().Border(0.4f, Unit.Point).Text(" " + item.value).Style(TextStyle.Default.FontSize(10)); // Display with a different style for non-numeric values
                                        }
                                        table.Cell().Border(0.4f, Unit.Point).Text(" " + item.unit).Style(TextStyle.Default.FontSize(10));
                                        table.Cell().Border(0.4f, Unit.Point).Text(" " + item.displayReading).Style(TextStyle.Default.FontSize(10));
                                        table.Cell().Border(0.4f, Unit.Point).Text(" " + item.testMethod).Style(TextStyle.Default.FontSize(10));
                                    }
                                });
                            }

                        });

                        page.Footer().Height(2.5f, Unit.Centimetre)
                           .Column(column =>
                           {
                               column.Item().Table(table =>
                               {
                                   table.ColumnsDefinition(columns =>
                                   {
                                       columns.RelativeColumn();
                                       columns.RelativeColumn();
                                       columns.RelativeColumn();
                                   });
                                   table.Cell().AlignLeft().Height(2.0f, Unit.Centimetre).Width(2.0f, Unit.Centimetre).Image(GenerateQrCode(Reportdata[0].workOrderId));
                                   //  table.Cell().AlignLeft().Height(2.0f, Unit.Centimetre).Width(5.0f, Unit.Centimetre).Image(GenerateBarcode(Reportdata[0].workOrderId));
                                   table.Cell().AlignLeft().Height(2.0f, Unit.Centimetre).Width(5.0f, Unit.Centimetre).Text("");

                                   table.Cell().AlignRight().AlignBottom().Text(text =>
                                   {
                                       text.DefaultTextStyle(x => x.FontSize(8));
                                       text.CurrentPageNumber();
                                       text.Span(" of ");
                                       text.TotalPages();
                                   });
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


        public byte[] GetPatientReportType2(string TestId)
        {
            var testIds = TestId.Split(',').Select(int.Parse).ToList(); // Split TestId into a list of integers

            // Query to fetch report data from multiple tables
            var Reportdata = (from tb in db.tnx_Booking
                              join tbp in db.tnx_BookingPatient on tb.patientId equals tbp.patientId
                              join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                              join tos in db.tnx_Observations on tbi.id equals tos.testId
                              join tm in db.titleMaster on tb.title_id equals tm.id
                              join cm in db.centreMaster on tb.centreId equals cm.centreId
                              join da in db.doctorApprovalMaster on tbi.approvalDoctor equals da.doctorId
                              where testIds.Contains(tbi.id) && tos.showInReport == 1
                              select new
                              {
                                  da.signature,
                                  tb.workOrderId,
                                  BookingDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"), // Date formatting
                                  tm.title,
                                  tb.name,
                                  Age = tb.ageYear + "Y " + tb.ageMonth + "M " + tb.ageDay + "D",
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
                                  tbi.sampleTypeName,
                                  tos.isBold,
                                  SampleCollectionDate = tbi.sampleCollectionDate,
                                  ResultDate = tbi.resultDate
                              }).ToList();

            // If no report data found, return empty byte array
            if (Reportdata.Count > 0)
            {
                HiQPdf.HtmlToPdf htmlToPdfConverter = new HiQPdf.HtmlToPdf();
                var headerSpace = 200;
                var left = 30;
                var right = 30;
                StringBuilder htmlContent = new StringBuilder();

                // Define header template with placeholders for dynamic data
                var Header = db.labReportHeader.Where(l => l.isActive == 1).Select(l => l.headerCSS.ToString()).First();
                string investigationname = "";
                htmlContent.AppendFormat("<div style='margin-left:{0}px; margin-right:{1}px'>",  left, right);

                foreach (var item in Reportdata)
                {
                    if (investigationname == "" || investigationname != item.investigationName)
                    {
                        if (!string.IsNullOrEmpty(investigationname))
                        {
                            htmlContent.AppendFormat("<div><img src='data:{0}' alt='Image' /></div>",item.signature);
                            htmlContent.Append("<div style='page-break-before:always;'></div>"); 
                        }
                        var headerHtml = string.Format(Header,
                            item.workOrderId,
                            item.name,
                            item.name,
                            item.Age,
                            "Dr. X", // Replace with actual data for referring doctor
                            item.companyName,
                            item.BookingDate,
                            item.SampleCollectionDate,
                            item.ResultDate,
                            item.ResultDate,
                            item.centrecode);

                        htmlContent.AppendFormat("<div style='height:{0}px; margin-left:{1}px; margin-right:{2}px'></div>", headerSpace, left, right);
                        htmlContent.Append(headerHtml); 
                    }
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table style='width:100%'>");
                    if (investigationname != item.investigationName)
                    {

                        sb.AppendFormat("<tr><td colspan=4 >{0}</td></tr>", item.departmentName);
                        sb.Append("<tr><td style='width:40%' >TestName</td><td style='width:20%'>Value</td><td style='width:20%'>Unit</td><td style='width:20%' >Display Reading</td></tr>");
                        investigationname = item.investigationName;
                    }
                    sb.AppendFormat("<tr><td style='width:40%'>{0}</td><td style='width:20%' >{1}</td><td style='width:20%'>{2}</td><td style='width:20%' >{3}</td></tr>", item.observationName, item.value, item.unit, item.displayReading);
                    htmlContent.Append(sb.ToString());
                    htmlContent.Append("</div>");
                }
                //string footerHtml = @" <div style='text-align:center; font-size:12px; margin-top:30px; margin-bottom:20px;'> <hr /><p>Powered by MyLab - 2025</p></div>";
                //htmlContent.Append(footerHtml);

                // Convert HTML content to PDF
                byte[] pdfBytes = htmlToPdfConverter.ConvertHtmlToMemory(htmlContent.ToString(), null);
                return pdfBytes; 
            }
            else
            {
                return new byte[0]; 
            }
        }

        public static byte[] GenerateQrCode(string text)
        {
            using var qrGenrater = new QRCodeGenerator();
            using var qrCodeData = qrGenrater.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(10);
        }
        public static byte[] GenerateBarcode(string text)
        {
            GeneratedBarcode barcode = BarcodeWriter.CreateBarcode(text, BarcodeEncoding.Code128, 600, 180);
            var barcodeimage = barcode.ToPngBinaryData();
            return barcodeimage;
        }
        public byte[] GetPatientReportType3(string TestId)
        {

            var testIds = TestId.Split(',').Select(int.Parse).ToList();
            var Reportdata = (from tb in db.tnx_Booking
                              join tbp in db.tnx_BookingPatient on tb.patientId equals tbp.patientId
                              join tbi in db.tnx_BookingItem on tb.workOrderId equals tbi.workOrderId
                              join tos in db.tnx_Observations on tbi.id equals tos.testId
                              join tm in db.titleMaster on tb.title_id equals tm.id
                              join cm in db.centreMaster on tb.centreId equals cm.centreId
                              where testIds.Contains(tbi.id) && tos.showInReport == 1
                              select new
                              {
                                  tb.workOrderId,
                                  BookingDate = tb.bookingDate.ToString("yyyy-MMM-dd hh:mm tt"),
                                  tm.title,
                                  tb.name,
                                  Age = tb.ageYear + "Y " + tb.ageMonth + "M " + tb.ageDay + "D",
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
                                  SampleCollectionDate = tbi.sampleCollectionDate,
                                  ResultDate = tbi.resultDate
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
                        page.Foreground();
                        page.DefaultTextStyle(x => x.FontFamily("TimesNewRoman"));
                        page.DefaultTextStyle(x => x.FontSize(10));
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

                                    table.Cell().ColumnSpan(3).Border(0.8f, Unit.Point).Element(innerTable =>
                                    {
                                        innerTable.Table(subTable =>
                                        {
                                            subTable.ColumnsDefinition(columns =>
                                            {
                                                columns.ConstantColumn(3.3f, Unit.Centimetre);
                                                columns.ConstantColumn(6.0f, Unit.Centimetre);
                                            });
                                            subTable.Cell().Text(" Patient Name").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": " + Reportdata[0].name).Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(" Age/Gender").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": " + Reportdata[0].Age + "/" + ": " + Reportdata[0].gender).Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(" Ref Doctor").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": ").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(" Barcode No").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": " + Reportdata[0].barcodeNo).Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(" Visit Id").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": " + Reportdata[0].workOrderId).Style(TextStyle.Default.FontSize(10));
                                        });
                                    });
                                    table.Cell().ColumnSpan(3).Border(0.8f, Unit.Point).Element(innerTable =>
                                    {
                                        innerTable.Table(subTable =>
                                        {
                                            subTable.ColumnsDefinition(columns =>
                                            {
                                                columns.ConstantColumn(3.3f, Unit.Centimetre);
                                                columns.ConstantColumn(6.0f, Unit.Centimetre);
                                            });
                                            subTable.Cell().Text(" Collected").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": " + Reportdata[0].SampleCollectionDate).Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(" Received").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": " + Reportdata[0].SampleCollectionDate).Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(" Reported").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": " + Reportdata[0].ResultDate).Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(" Client Code").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": " + Reportdata[0].centrecode).Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(" Client Name").Style(TextStyle.Default.FontSize(10));
                                            subTable.Cell().Text(": " + Reportdata[0].companyName).Style(TextStyle.Default.FontSize(10));
                                        });
                                    });

                                    table.Cell().ColumnSpan(6).Text("").Style(TextStyle.Default.FontSize(6));
                                });
                        });

                        page.Content()
                        .Column(column =>
                        {
                            string departmentName = "";

                            foreach (var item in Reportdata)
                            {
                                if (item.departmentName != departmentName && !string.IsNullOrEmpty(departmentName))
                                {
                                    column.Item().Element(e => e.PageBreak()); // Add page break between departments
                                }

                                if (item.departmentName != departmentName)
                                {
                                    column.Item().Table(table =>
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

                                        table.Cell().ColumnSpan(11).Height(0.5f, Unit.Centimetre).Border(0.4f, Unit.Point).Text(item.departmentName).Style(TextStyle.Default.FontSize(10).Bold()).AlignCenter();
                                        //result data Header
                                        table.Cell().ColumnSpan(3).Height(0.5f, Unit.Centimetre).Border(0.4f, Unit.Point).Text(" Test Name").Style(TextStyle.Default.FontSize(10).Bold());
                                        table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Border(0.4f, Unit.Point).Text(" Result").Style(TextStyle.Default.FontSize(10).Bold());
                                        table.Cell().ColumnSpan(1).Height(0.5f, Unit.Centimetre).Border(0.4f, Unit.Point).Text(" Unit").Style(TextStyle.Default.FontSize(10).Bold());
                                        table.Cell().ColumnSpan(3).Height(0.5f, Unit.Centimetre).Border(0.4f, Unit.Point).Text(" Bio. Ref. Range").Style(TextStyle.Default.FontSize(10).Bold());
                                        table.Cell().ColumnSpan(2).Height(0.5f, Unit.Centimetre).Border(0.4f, Unit.Point).Text(" Method Name").Style(TextStyle.Default.FontSize(10).Bold());
                                        // end here
                                        table.Cell().ColumnSpan(11).Height(0.5f, Unit.Centimetre).Text("").Style(TextStyle.Default.FontSize(10)).AlignCenter();

                                        departmentName = item.departmentName;
                                    });
                                }

                                column.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(3);
                                        columns.RelativeColumn(2);
                                        columns.RelativeColumn(1);
                                        columns.RelativeColumn(3);
                                        columns.RelativeColumn(2);
                                    });

                                    if (item.isHeader == 1)
                                    {
                                        table.Cell().ColumnSpan(5).PaddingLeft(1.8f, Unit.Point).Height(0.5f, Unit.Centimetre).Text(item.investigationName).Style(TextStyle.Default.FontSize(10).Bold()).AlignLeft();
                                    }
                                    else
                                    {
                                        table.Cell().Text(item.observationName).Style(TextStyle.Default.FontSize(10));
                                        if (double.TryParse(item.value, out double numericValue))
                                        {
                                            if (numericValue < item.min || numericValue > item.max)
                                                table.Cell().Text(item.value).Style(TextStyle.Default.FontSize(10).Bold().FontColor(QuestPDF.Infrastructure.Color.FromHex("#B20000"))); // Highlight abnormal values
                                            else
                                                table.Cell().Text(item.value).Style(TextStyle.Default.FontSize(10)); // Normal style for values within the range
                                        }
                                        else
                                        {
                                            table.Cell().Text(item.value).Style(TextStyle.Default.FontSize(10)); // Display with a different style for non-numeric values
                                        }
                                        table.Cell().Text(item.unit).Style(TextStyle.Default.FontSize(10));
                                        table.Cell().Text(item.displayReading).Style(TextStyle.Default.FontSize(10));
                                        table.Cell().Text(item.testMethod).Style(TextStyle.Default.FontSize(10));
                                    }
                                });
                            }

                        });

                        page.Footer().Height(2.5f, Unit.Centimetre)
                           .Column(column =>
                           {
                               column.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                    });
                                    table.Cell().AlignLeft().Height(2.0f, Unit.Centimetre).Width(2.0f, Unit.Centimetre).Image(GenerateQrCode(Reportdata[0].workOrderId));
                                    table.Cell().AlignLeft().Height(2.0f, Unit.Centimetre).Width(5.0f, Unit.Centimetre).Image(GenerateBarcode(Reportdata[0].workOrderId));

                                    table.Cell().AlignRight().AlignBottom().Text(text =>
                                    {
                                        text.DefaultTextStyle(x => x.FontSize(8));
                                        text.CurrentPageNumber();
                                        text.Span(" of ");
                                        text.TotalPages();
                                    });
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

        async Task<ServiceStatusResponseModel> IPatientReportServices.ReportHoldUnHold(string TestId, int isHold, int holdBy, string holdReason)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var testIds = TestId.Split(',').Select(int.Parse).ToList();
                    var testdata = db.tnx_BookingItem.Where(b => testIds.Contains(b.id)).ToList();
                    if (testIds.Count == 0)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "No data found to hold"
                        };
                    }
                    foreach (var test in testdata)
                    {
                        if (isHold == 1)
                        {
                            test.hold = isHold;
                            test.holdById = holdBy;
                            test.holdDate = DateTime.Now;
                            test.holdReason = holdReason;
                        }
                        else
                        {
                            test.hold = isHold;
                            test.UnholdById = holdBy;
                            test.unHoldDate = DateTime.Now;
                        }
                    }
                    db.tnx_BookingItem.UpdateRange(testdata);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Updated Successful"
                    };

                }
                catch (Exception ex)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message,
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> IPatientReportServices.ReportNotApprove(string TestId, string userid)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var testIds = TestId.Split(',').Select(int.Parse).ToList();
                    var testdata = db.tnx_BookingItem.Where(b => testIds.Contains(b.id) && b.isApproved == 1).ToList();
                    if (testIds.Count == 0)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "No data found to unApprove"
                        };
                    }
                    foreach (var test in testdata)
                    {
                        test.isApproved = 0;
                        test.notApprovedBy = userid;
                        test.notApprovedDate = DateTime.Now;

                    }
                    db.tnx_BookingItem.UpdateRange(testdata);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "UnApproved Successful"
                    };

                }
                catch (Exception ex)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message,
                    };
                }
            }

        }
    }
}
