using OfficeOpenXml.Table;
using OfficeOpenXml;

namespace iMARSARLIMS
{
    public class MyFunction
    {
        public static byte[] ExportToExcel<T>(List<T> Table, String FileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using (var package = new ExcelPackage())
            {
                // Add a worksheet to the package
                var worksheet = package.Workbook.Worksheets.Add(FileName);

                // Load the list of objects (table) into the worksheet starting from the top-left corner (A1)
                worksheet.Cells["A1"].LoadFromCollection(Table, true, TableStyles.Light1);

                // Optionally, style the header row
                var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column];
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerRow.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                return package.GetAsByteArray();
            }
        }

    }
}
