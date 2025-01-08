using iMARSARLIMS.Interface;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace iMARSARLIMS.Services
{
    public class PatientReportServices : IPatientReportServices
    {
        public byte[] GetPatientReport(string TestId)
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
                    page.DefaultTextStyle(x => x.FontSize(20));

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
                                table.Cell().BorderLeft(1.0f,Unit.Point).BorderTop(1.0f, Unit.Point).Text(" Patient Name").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(2).BorderTop(1.0f, Unit.Point).Text(": Shubham Tiwari").Style(TextStyle.Default.FontSize(10));
                                table.Cell().BorderLeft(1.0f, Unit.Point).BorderTop(1.0f, Unit.Point).Text(" Collected").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(2).BorderRight(1.0f, Unit.Point).BorderTop(1.0f, Unit.Point).Text(": 07-Jan-2025 15:45 PM").Style(TextStyle.Default.FontSize(10));
                                table.Cell().BorderLeft(1.0f, Unit.Point).Text(" Age/Gender").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(2).Text(": 10 Y 1 M 0 D / Male").Style(TextStyle.Default.FontSize(10));
                                table.Cell().BorderLeft(1.0f, Unit.Point).Text(" Received").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(2).BorderRight(1.0f, Unit.Point).Text(": 07-Jan-2025 15:45 PM").Style(TextStyle.Default.FontSize(10));
                                table.Cell().BorderLeft(1.0f, Unit.Point).Text(" Ref Doctor").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(2).Text(": Dr.Ladmanand Maurya").Style(TextStyle.Default.FontSize(10));
                                table.Cell().BorderLeft(1.0f, Unit.Point).Text(" Reported").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(2).BorderRight(1.0f, Unit.Point).Text(": 07-Jan-2025 15:45 PM").Style(TextStyle.Default.FontSize(10));
                                table.Cell().BorderLeft(1.0f, Unit.Point).Text(" Barcode No").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(2).Text(": 12345688").Style(TextStyle.Default.FontSize(10));
                                table.Cell().BorderLeft(1.0f, Unit.Point).Text(" Client Code").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(2).BorderRight(1.0f, Unit.Point).Text(": hjlg456").Style(TextStyle.Default.FontSize(10));
                                table.Cell().BorderLeft(1.0f, Unit.Point).BorderBottom(1.0f, Unit.Point).Text(" Visit Id").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(2).BorderBottom(1.0f, Unit.Point).Text(": 15677").Style(TextStyle.Default.FontSize(10));
                                table.Cell().BorderLeft(1.0f, Unit.Point).BorderBottom(1.0f, Unit.Point).Text(" Client Name").Style(TextStyle.Default.FontSize(10));
                                table.Cell().ColumnSpan(2).BorderBottom(1.0f, Unit.Point).BorderRight(1.0f, Unit.Point).Text(": Pikachu Collection centre, noida").Style(TextStyle.Default.FontSize(10));
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
                                table.Cell().ColumnSpan(3).Border(0.8f, Unit.Point).Text("Test Name").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().ColumnSpan(2).Border(0.8f, Unit.Point).Text("Result").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().ColumnSpan(1).Border(0.8f, Unit.Point).Text("Unit").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().ColumnSpan(3).Border(0.8f, Unit.Point).Text("Bio. Ref. Range").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().ColumnSpan(2).Border(0.8f, Unit.Point).Text("Method Name").Style(TextStyle.Default.FontSize(10).Bold());
                                table.Cell().ColumnSpan(11).Border(0.8f, Unit.Point).Text("Department of").Style(TextStyle.Default.FontSize(10).Bold()).AlignCenter();
                                //for (int i = 1; i <= 10; i++)
                                //{
                                //    table.Cell().Text("" + i).Style(TextStyle.Default.FontSize(10));
                                //    table.Cell().Text("Service Name" + i).Style(TextStyle.Default.FontSize(10));
                                //    table.Cell().Text("DepartMent" + i).Style(TextStyle.Default.FontSize(10));
                                //    table.Cell().Text("Rate" + i).Style(TextStyle.Default.FontSize(10));
                                //    table.Cell().Text("Discount" + i).Style(TextStyle.Default.FontSize(10));
                                //    table.Cell().Text("Total" + i).Style(TextStyle.Default.FontSize(10));
                                //}

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
    }
}
