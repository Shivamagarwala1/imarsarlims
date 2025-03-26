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
        private static readonly string[] ones = new string[] { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        private static readonly string[] tens = new string[] { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        private static readonly string[] thousands = new string[] { "", "Thousand", "Million", "Billion" };

        public static string AmountToWord(decimal number)
        {
            if (number == 0)
                return "Zero only";

            string words = "";
            int partCounter = 0;
            while (number > 0)
            {
                int part = (int)(number % 1000);
                if (part > 0)
                {
                    string partWords = ConvertPart(part);
                    words = partWords + (thousands[partCounter] != "" ? " " + thousands[partCounter] : "") + " " + words;
                }
                number /= 1000;
                partCounter++;
            }

            return words.Trim() + " only";
        }

        // Helper method to convert each part (less than 1000) to words
        private static string ConvertPart(int number)
        {
            if (number == 0) return "";

            if (number < 20)
                return ones[number];

            if (number < 100)
                return tens[number / 10] + (number % 10 != 0 ? " " + ones[number % 10] : "");

            return ones[number / 100] + " Hundred" + (number % 100 != 0 ? " " + ConvertPart(number % 100) : "");
        }

    }
}
