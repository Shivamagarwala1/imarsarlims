namespace iMARSARLIMS.Request_Model
{
    public class ReportLetterHead
    {
        public int CentreId { get; set; }
        public int reporrtHeaderHeightY { get; set; }
        public int patientYHeader { get; set; }
        public int barcodeXPosition { get; set; }
        public int barcodeYPosition { get; set; }
        public int QRCodeXPosition { get; set; }
        public int QRCodeYPosition { get; set; }
        public int isQRheader { get; set; }
        public int isBarcodeHeader { get; set; }
        public int footerHeight { get; set; }
        public int NABLxPosition { get; set; }
        public int NABLyPosition { get; set; }
        public int docSignYPosition { get; set; }
        public int receiptHeaderY { get; set; }
        public string? reportHeader { get; set; }
        public string? reciptHeader { get; set; }
        public string? reciptFooter { get; set; }
        public string? WaterMarkImage { get; set; }
        public string? NablImage { get; set; }

    }
}
